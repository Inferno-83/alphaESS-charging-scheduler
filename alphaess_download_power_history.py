from datetime import datetime, timedelta
import logging
import os 
import sys
import math
import csv
import time
import json
import requests
import argparse
from zoneinfo import ZoneInfo
import asyncio
from enum import verify
from alphaess.alphaess import alphaess
import pandas as pd

async def main():
    parser = argparse.ArgumentParser(description="Download AlphaESS data for a date range")
    parser.add_argument("--app-id", required=True, help="AlphaESS App ID")
    parser.add_argument("--app-secret", required=True, help="AlphaESS App Secret")
    parser.add_argument("--serial", required=True, help="System serial number")

    parser.add_argument(
        "--output-path",
        type=str,
        default="./",
        help="Path to output data to"
    )

    parser.add_argument(
        "--from-days-ago",
        type=int,
        default=7,
        help="Start of range (days ago, older date)"
    )

    parser.add_argument(
        "--to-days-ago",
        type=int,
        default=1,
        help="End of range (days ago, newer date)"
    )

    args = parser.parse_args()

    FROM_DAYS_AGO = args.from_days_ago
    TO_DAYS_AGO = args.to_days_ago
    OUTPUT_PATH = args.output_path

    os.makedirs(f"{OUTPUT_PATH}/log", exist_ok=True)

    log_filename = f"{OUTPUT_PATH}/log/alphaess_download_{datetime.now().strftime('%Y-%m-%d')}.log"

    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s [%(levelname)s] %(message)s",
        handlers=[
            logging.FileHandler(log_filename),
            logging.StreamHandler(sys.stdout)
        ]
    )

    if FROM_DAYS_AGO < TO_DAYS_AGO:
        logging.error("--from-days-ago must be >= --to-days-ago")
        sys.exit(1)

    root_path = f"{OUTPUT_PATH}/data"

    now = datetime.now()
    start_date = (now - timedelta(days=FROM_DAYS_AGO)).replace(
        hour=0, minute=0, second=0, microsecond=0
    )

    end_date = (now - timedelta(days=TO_DAYS_AGO)).replace(
        hour=0, minute=0, second=0, microsecond=0
    )
    total_days = (end_date - start_date).days + 1

    tz = ZoneInfo("Australia/Perth")

    logging.info("Logging in...")

    api = alphaess(args.app_id, args.app_secret)

    # Create output folder per inverter
    base_path = os.path.join(root_path, args.serial)
    os.makedirs(base_path, exist_ok=True)

    logging.info(f"Processing battery: {args.serial}")

    for i in range(total_days):
        day = start_date + timedelta(days=i)
        date_str = day.strftime("%Y-%m-%d")

        filename = f"battery_power_history_{date_str}.csv"
        filepath = os.path.join(base_path, filename)

        # Skip if already exists
        if os.path.exists(filepath):
            logging.warning(f"Skipping {filename} (already exists)")
            continue

        logging.info(f"Downloading {day.date()}")

        try:
            powerData = await api.getOneDayPowerBySn(args.serial, date_str)

            if powerData is not None:
                for entry in powerData:
                    entry["date"] = date_str
        except Exception as e:
            logging.error(f"Error fetching {date_str}: {e}")

        if powerData is None or len(powerData) == 0:
            logging.warning(f"    No data for {day.date()}")
            continue

        # Write CSV per day
        logging.info(f"    Saving {len(powerData)} rows to {filename}")

        with open(filepath, "w", newline="") as f:
            writer = csv.writer(f)
            writer.writerow(["time", "ppv", "load", "cbat", "feedIn", "gridCharge", "chargingPile"])

            for row in powerData:
                writer.writerow([
                    row.get("uploadTime"),
                    row.get("ppv", ""),
                    row.get("load", ""),
                    row.get("cbat", ""),
                    row.get("feedIn", ""),
                    row.get("gridCharge", ""),
                    row.get("pchargingPile", "")
                ])

    logging.info("Done.")
    await api.close()

# This starts the event loop and runs the main coroutine
if __name__ == '__main__':
    asyncio.run(main())
