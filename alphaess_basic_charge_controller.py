#!/usr/bin/env python3

from datetime import time, datetime, timedelta
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

def parse_time(value: str):
    try:
        return datetime.strptime(value, "%H:%M").time()
    except ValueError:
        raise argparse.ArgumentTypeError(
            f"Invalid time format: '{value}'. Expected HH:MM"
        )

async def main():

    parser = argparse.ArgumentParser(description="Download AlphaESS data for a date range")
    parser.add_argument("--app-id", required=True, help="AlphaESS App ID")
    parser.add_argument("--app-secret", required=True, help="AlphaESS App Secret")
    parser.add_argument("--serial", required=True, help="System serial number")


    parser.add_argument(
        "--start",
        type=parse_time,
        default=parse_time("12:00"),
        help="Allowed start time of the charge schedule"
    )

    parser.add_argument(
        "--end",
        type=parse_time,
        default=parse_time("15:00"),
        help="Allowed end time of the charge schedule"
    )

    parser.add_argument(
        "--threshold",
        type=float,
        default=90,
        help="If battery falls below threshold, charging will turn on"
    )

    parser.add_argument(
        "--output-path",
        type=str,
        default="./",
        help="Path to output data to"
    )

    args = parser.parse_args()

    # We always charge to 100%
    SOC_LEVEL = 100
    SOC_THRESHOLD = args.threshold
    CHARGE_START = args.start
    CHARGE_END = args.end
    OUTPUT_PATH = args.output_path

    # -------------------------------------------------------------------------
    # CONNECT
    # -------------------------------------------------------------------------

    print(f"----------------------------------")
    print(print(datetime.now()))
    print(f"Checking System...")
    api = alphaess(args.app_id, args.app_secret)

    # -------------------------------------------------------------------------
    # GET CURRENT BATTERY STATUS
    # -------------------------------------------------------------------------

    try:
        data = await api.getLastPowerData(args.serial)

        soc = float(data["soc"])

        print(f"Current battery SOC: {soc}%")

    except Exception as ex:
        print(f"Failed to retrieve battery data: {ex}")
        await api.close()
        return

    chargeConfig = await api.getChargeConfigInfo(args.serial);

    isChargeEnabled = chargeConfig.get("gridCharge", False)

    print(f"Charging config: {chargeConfig}")

    print(f"Charging enabled: {isChargeEnabled}")

    now = datetime.now().time()
    print(f"----------------------------------")
    print(f"Analysing state...")
    
    if CHARGE_START <= now <= CHARGE_END:
        print(f"Current time {now} is within the schedule window [{CHARGE_START}, {CHARGE_END}]");

        if soc < SOC_THRESHOLD:
            if isChargeEnabled:
                print(f"SOC below {SOC_THRESHOLD}%")
                setSchedule = False
            else:
                print(f"SOC below {SOC_THRESHOLD}%, enabling charge schedule...")
                setSchedule = True
                CHARGE_ENABLED = 1

        else:
            if isChargeEnabled:
                print(f"SOC above {SOC_THRESHOLD}%, disabling charge schedule...")
                setSchedule = True
                CHARGE_ENABLED = 0
            else:
                print(f"SOC above {SOC_THRESHOLD}%")
                setSchedule = False
    else:
        print(f"Current time {now} is outside the schedule window [{CHARGE_START}, {CHARGE_END}]");
        if isChargeEnabled:
            print("Disabling charge schedule...")
            setSchedule = True
            CHARGE_ENABLED = 0
        else:
            setSchedule = False

    # -------------------------------------------------------------------------
    # ENABLE CHARGE SCHEDULE IF BELOW THRESHOLD
    # -------------------------------------------------------------------------

    print(f"----------------------------------")
    if setSchedule:

        try:
            print(f"Updating...")
            timeChaf1 = CHARGE_START.strftime("%H:%M")
            timeChae1 = CHARGE_END.strftime("%H:%M")
            timeChaf2 = chargeConfig.get("timeChaf2")
            timeChae2 = chargeConfig.get("timeChae2")
            
            result = await api.updateChargeConfigInfo(
                    args.serial, 
                    SOC_LEVEL,
                    CHARGE_ENABLED,
                    timeChae1,
                    timeChae2,
                    timeChaf1,
                    timeChaf2)

            print(f"Charge schedule enabled state successfully set to 'gridCharge: {CHARGE_ENABLED} timeChaf1: {timeChaf1} timeChae1: {timeChae1} timeChaf2: {timeChaf2} timeChae2: {timeChae2}'. Result:{result}")

            chargeConfig = await api.getChargeConfigInfo(args.serial);

            print(f"Updated config: {chargeConfig}")

        except Exception as ex:
            print(f"Failed to change charge schedule: {ex}")

    else:
        print("No changes required!")

    print(f"----------------------------------")
    print("Done.")
    print(f"----------------------------------")
    await api.close()

if __name__ == "__main__":
    asyncio.run(main())
