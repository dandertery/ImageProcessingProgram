using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.ML;
using System.Globalization;
using System.Diagnostics;
using ScottPlot;

namespace ImageProcessingProgram
{
    public class SpectralImages : Mat
    {
        private string filename;
        private int SY, SX, FY, FX;
        Mat[] specFrames = new Mat[32];


        public  SpectralImages(string Filename, int[] sfArray)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            filename = Filename;
            SY = sfArray[0];
            SX = sfArray[1];
            FY = sfArray[2];
            FX = sfArray[3];



            for (int Frame = 0; Frame < 32; Frame++)
            {
                specFrames[Frame] = new Mat(Path.Combine(projectDirectory, "blank.jpg"), ImreadModes.Color);
            }
            WriteToArray(LoadImages(filename));


        }
        public Mat[] GetArray()
        {
            return specFrames;
        }
        public double[] GetIntensity(Mat[] input, int pY, int pX)
        {
            double[] pixIntensities = new double[32];
            for (int spectral = 0; spectral < 32; spectral++)
            {
                Vec3b color = input[spectral].Get<Vec3b>(pY, pX);
                pixIntensities[spectral] = color.Item0;

                
            }
            return pixIntensities;

        }
        public double[] GetMaxIntensity(Mat[] input)
        {
            return MaxIntensity(input);
        }
        public double[] GetMinIntensity(Mat[] input)
        {
            return MinIntensity(input);
        }
        public double[] GetMeanIntensity(Mat[] input)
        {
            return MeanIntensity(input);
        }
        public double[] GetPeakSNR(Mat[] input)
        {
            double[] SNRArray = new double[32];
            double[] SDArray = GetSD(input, SY, SX, FY, FX);
            double[] MIntensityArray = GetMaxIntensity(input);
            for (int spectral = 0; spectral < 32; spectral++)
            {
                //Debug.WriteLine(MIntensityArray[spectral] + " / " + SDArray[spectral] + " = " + MIntensityArray[spectral] / SDArray[spectral]);
                SNRArray[spectral] = MIntensityArray[spectral] / SDArray[spectral];
                SNRArray[spectral] = Math.Round(SNRArray[spectral], 4);
            }
            //Debug.WriteLine("Break");
            return SNRArray;

        }
        public double[] GetMeanSNR(Mat[] input)
        {
            double[] SNRArray = new double[32];
            double[] SDArray = GetSD(input, SY, SX, FY, FX);
            double[] MIntensityArray = GetMeanIntensity(input);
            for (int spectral = 0; spectral < 32; spectral++)
            {
                //Debug.WriteLine(MIntensityArray[spectral] + " / " + SDArray[spectral] + " = " + MIntensityArray[spectral] / SDArray[spectral]);
                SNRArray[spectral] = MIntensityArray[spectral] / SDArray[spectral];
                SNRArray[spectral] = Math.Round(SNRArray[spectral], 4);
            }
            //Debug.WriteLine("Break");
            return SNRArray;

        }
        public double[] GetSD(Mat[] bInput, int sY, int sX, int fY, int fX)
        {
            double[] bgSD = new double[32];
            double tempSD = 0;
            double SD;
            double tempAv = 0;

            double[] pixInt = new double[(fY - sY) * (fX - sX)];

            int i = 0;
            for (int spectral = 0; spectral < 32; spectral++)
            {
                
                for (int x = sX; x < fX; x++)
                {
                    for (int y = sY; y < fY; y++)
                    {
                        Vec3b color = bInput[spectral].Get<Vec3b>(x, y);
                        pixInt[i] = color.Item0;
                        i++;
                    }

                }
                i = 0;
                for (int s = 0; s < pixInt.Length; s++)
                {
                    tempAv = tempAv + pixInt[s];
                }
                double average = tempAv / pixInt.Length;
                for (int q = 0; q < pixInt.Length; q++)
                {
                    tempSD = tempSD + ((pixInt[q] - average) * (pixInt[q] - average));
                }
                tempSD = tempSD / pixInt.Length;
                SD = Math.Sqrt(tempSD);
                bgSD[spectral] = SD;
                tempAv = 0;
                tempSD = 0;




            }

            return bgSD;
        }
        private decimal[,,] LoadImages(string filename)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string FilePath = "C:/Users/Archie/Documents/IPP/Image Processing Program public/ImageProcessingProgram/orig_images/" + filename; //should generalise this for program to work in any directory (eg use of Path.Combine())
            
            string[] text = File.ReadAllLines(FilePath);
            decimal[] text2 = new decimal[text.Length - 7];  //0,1,2,3,4 redundant, last 3 lines empty 
            for (int i = 0; i < text2.Length; i++)
            {
                text2[i] = decimal.Parse(text[i + 5]);
            }

            decimal uLimit = 255; //normalising 0-255
            decimal max = 0;
            decimal min = 0;
            for (int i = 0; i < text2.Length; i++)
            {
                if (text2[i] > max)
                {
                    max = text2[i];
                }
            }

            for (int i = 0; i < text2.Length; i++)
            {
                if (text2[i] < min)
                {
                    min = text2[i];
                }
            }
            decimal range = max - min;
            for (int i = 0; i < text2.Length; i++)
            {
                decimal rangedValue = (text2[i] - min) / range;
                text2[i] = uLimit * rangedValue; //lLimit being 0
            }

            decimal[,,] fileArray = new decimal[32, 286, 286];
            int z = 0;

            for (int y = 0; y < 286; y++)
            {
                for (int x = 0; x < 286; x++)
                {
                    for (int spectral = 0; spectral < 32; spectral++)
                    {

                        fileArray[spectral, x, y] = text2[z];
                        z++;
                    }
                }

            }
            return fileArray;
        }
        private void WriteToArray(decimal[,,] dataArray)
        {
            for (int spectral = 0; spectral < 32; spectral++) //writing grayscale
            {
                for (int x = 0; x < 286; x++)
                {
                    for (int y = 0; y < 286; y++)
                    {

                        decimal s = (dataArray[spectral, x, y]);
                        Vec3b color = new Vec3b();
                        byte p = (byte)(s);

                        color.Item0 = p;
                        color.Item1 = p;
                        color.Item2 = p;
                        specFrames[spectral].Set<Vec3b>(x, y, color);
                        int w = 0;

                    }
                }
            }
        }
        private double[] MaxIntensity(Mat[] finput) //called 3 times
        {
            double maxIntensity = 0;
            double[] maxInt = new double[32];
            for (int spectral = 0; spectral < 32; spectral++) //writing grayscale
            {
                for (int x = 0; x < 286; x++)
                {
                    for (int y = 0; y < 286; y++)
                    {
                        Vec3b color = finput[spectral].Get<Vec3b>(y, x);
                        if(color.Item0>maxIntensity)
                        {
                            maxIntensity = color.Item0;
                        }

                    }
                }
                maxInt[spectral] = maxIntensity;
                maxIntensity = 0;
            }
            return maxInt;
        }
        private double[] MeanIntensity(Mat[] finput) //called 3 times
        {
            double meanTemp = 0;
            double[] meanInt = new double[32];
            for (int spectral = 0; spectral < 32; spectral++) 
            {
                for (int x = 0; x < 286; x++)
                {
                    for (int y = 0; y < 286; y++)
                    {
                        Vec3b color = finput[spectral].Get<Vec3b>(y, x);
                        meanTemp = meanTemp + color.Item0;
                    }
                }
                meanInt[spectral] = meanTemp/(286*286);
                meanTemp = 0;
            }
            return meanInt;
        }
        private double[] MinIntensity(Mat[] Input)
        {
            double minIntensity = 255;
            double[] minInt = new double[32];
            for (int spectral = 0; spectral < 32; spectral++) //writing grayscale
            {
                for (int x = 0; x < 286; x++)
                {
                    for (int y = 0; y < 286; y++)
                    {
                        Vec3b color = Input[spectral].Get<Vec3b>(y, x);
                        if (color.Item0 < minIntensity)
                        {
                            minIntensity = color.Item0;
                        }

                    }
                }
                minInt[spectral] = minIntensity;
                minIntensity = 0;
            }
            return minInt;
        }
    }
}