//Copyright (C) 2009 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections;

namespace QuickImageComment
{
    public class Performance
    {
        // class to make one measurement and store values
        private class Measurement
        {
            public string Name = "";
            public DateTime MeasuredTime;

            public Measurement(string givenName)
            {
                Name = givenName;
                MeasuredTime = DateTime.Now;
            }
        }

        private ArrayList Measurements;
        private DateTime StartTime;

        // constructor, initialise sequence of measurements
        public Performance()
        {
            Measurements = new ArrayList();
            StartTime = DateTime.Now;
        }

        // make new measurement
        public void measure(string Name)
        {
            Measurements.Add(new Measurement(Name));
        }

        // display results of measurements in message box
        // check ConfigDefinition if measurement should be displayed
        public void display(ConfigDefinition.enumConfigFlags indexConfigFlag)
        {
            if (ConfigDefinition.getConfigFlag(indexConfigFlag))
            {
                string output = indexConfigFlag.ToString() + "\n";
                DateTime Reference = StartTime;
                DateTime EndTime = DateTime.Now;
                foreach (string measurementString in getMeasurements(indexConfigFlag))
                {
                    output = output + measurementString + "\n";
                }
                output = output + DateTime.Now.ToString("HH:mm:ss:fff");
                GeneralUtilities.debugMessage(output);
            }
        }

        // log results of measurements in logger
        // check ConfigDefinition if measurement should be displayed
        public void log(ConfigDefinition.enumConfigFlags indexConfigFlag)
        {
            if (ConfigDefinition.getConfigFlag(indexConfigFlag))
            {
                string output = indexConfigFlag.ToString() + "\r\n";
                DateTime Reference = StartTime;
                DateTime EndTime = DateTime.Now;
                foreach (string measurementString in getMeasurements(indexConfigFlag))
                {
                    output = output + measurementString + "\r\n";
                }
                output = output + DateTime.Now.ToString("HH:mm:ss:fff");
                Logger.log(output);  // permanent use of Logger.log
            }
        }

        // return measurements
        // check ConfigDefinition if measurement should be displayed
        public ArrayList getMeasurements(ConfigDefinition.enumConfigFlags indexConfigFlag)
        {
            ArrayList MeasurementStrings = new ArrayList();

            if (ConfigDefinition.getConfigFlag(indexConfigFlag))
            {
                DateTime Reference = StartTime;
                DateTime EndTime = DateTime.Now;
                foreach (Measurement aMeasurement in Measurements)
                {
                    string duration1 = aMeasurement.MeasuredTime.Subtract(StartTime).TotalMilliseconds.ToString("0");
                    if (duration1.Length < Logger.totalDigits)
                    {
                        duration1 = new string(' ', Logger.totalDigits * 2 - 2 * duration1.Length) + duration1;
                    }
                    string duration2 = aMeasurement.MeasuredTime.Subtract(Reference).TotalMilliseconds.ToString("0");
                    if (duration2.Length < Logger.diffDigits)
                    {
                        duration2 = new string(' ', Logger.diffDigits * 2 - 2 * duration2.Length) + duration2;
                    }
                    MeasurementStrings.Add(duration1 + "\t " + duration2 + "\t > " + aMeasurement.Name);
                    Reference = aMeasurement.MeasuredTime;
                }
                //MeasurementStrings.Add(EndTime.Subtract(StartTime).TotalMilliseconds.ToString("0") + " ms  Total for " + indexConfigFlag.ToString());
            }
            return MeasurementStrings;
        }
    }
}
