using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.ML;
using System.Globalization;
using System.Diagnostics;
using ScottPlot;
using ScottPlot.Drawing;

namespace ImageProcessingProgram
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // openCVSharp uses BGR
        {
            int Y1;
            int X1;

            int Y2;
            int X2;

            int Y3;
            int X3;

            int Y4;
            int X4;
            ///////////////////////////////////////////////////////////////////////////////
            int size = 286;
            int ksize = 15;
            
            double SigmaX = 80; // should be less than 4/5 for gaussian?
            string actualSigmaX;
            if(SigmaX == 0)
            {
                actualSigmaX = "AM";
            }
            else
            {
                actualSigmaX = SigmaX.ToString();
            }
            
            
            double SigmaY = 80;
            
            int imageNumber = 6; // 6, 10, 12
            //string testName = "spectral Gaussian," + " kernel " + ksize + ", SX " + actualSigmaX +  ", Image " + imageNumber + "";
            string testName = "Bilateral, d " + ksize + ", sS " + SigmaX +  " sC " + SigmaY + ", on image " + imageNumber;
            //string testName = "Spectral Median Blur, kernel size " + ksize + " image " + imageNumber + "  ";
            //string testName = "Original " + imageNumber; // if you want base image(s)
            
            string filename = "srs_" + imageNumber + ".txt"; // (6, 10, 12)
            string testInfo = testName; //+ " ksize " + ksize + " SX " + SigmaX + " SY " + SigmaY + "Image " + imageNumber;
            
            int[] bSquare = new int[4];
            var fontSize = 10;
            var alignment = Alignment.UpperLeft;
            if(imageNumber == 6)
            {
                Y1 = 100; // 230, 82
                X1 = 100;

                Y2 = 80;
                X2 = 12;

                Y3 = 209;
                X3 = 147;

                Y4 = 26;
                X4 = 275;
                bSquare[0] = 12;
                bSquare[1] = 246;
                bSquare[2] = 33;
                bSquare[3] = 271;

            }
            else if (imageNumber == 10)
            {
                fontSize = 11;
                alignment = Alignment.UpperRight;
                Y1 = 175;
                X1 = 89;

                Y2 = 63;
                X2 = 20;

                Y3 = 59;
                X3 = 227;

                Y4 = 135;
                X4 = 166;
                bSquare[0] = 34;
                bSquare[1] = 194;
                bSquare[2] = 107;
                bSquare[3] = 265;
            }
            else if (imageNumber == 12)
            {
                fontSize = 11;
                Y1 = 157;
                X1 = 211;

                Y2 = 100;
                X2 = 107;

                Y3 = 9;
                X3 = 77;

                Y4 = 267;
                X4 = 48;
                bSquare[0] = 231;
                bSquare[1] = 127;
                bSquare[2] = 282;
                bSquare[3] = 163;
            }
            else
            {
                throw new Exception("incorrect image number");
            }
            bool spatial = true;
            SpectralImages rmi = new SpectralImages(filename, bSquare); //string Filename, int sY, int sX, int fY, int fX
            Mat[] specFrames = rmi.GetArray();
            //Mat[] filteredspecFrames = specFrames; // if you want base image(s)
            Mat[] filteredspecFrames =  FilterChannels(specFrames, ksize, SigmaX, SigmaY, spatial);

            ///////////////////////////////////////////////////////////////////////////////


            //for 6                                                                                       
            //"background" from (246, 12) to (271, 33)  // ON GIMP, X,Y                                                                                          
            //interesting pixels: (82,225), (12, 80), (246,56), (147, 209)                                                                                      
            //for 10                                                                                          
            //"background" from (194, 34) to (265, 107)                                                                                         
            //interesting pixels: (89,175), (227, 59), (20, 63), (166, 135)                                                                                       
            //for 12                                                                                          
            //"background" from (127, 231) to (163,282)                                                                                         
            //interesting pixels: (211, 157), (77, 9 ), (107,100), (48, 267)






            //Use projectDirectory to get to etc C:\Users\Archie\Documents\IPP\Image Processing Program public\ImageProcessingProgram
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName; 
            string initialImagePath = Path.Combine(projectDirectory, "/initialimage.jpg");
            filenameLabel.Text = filename + " " + testName;
            DisplayChannels(specFrames, filteredspecFrames, filename, true); // display image set seperately


            //SNR each of the source and filtered channels
            double[] peakSNRArray = rmi.GetPeakSNR(specFrames); 
            double[] filteredPeakSNRArray = rmi.GetPeakSNR(filteredspecFrames);
            double[] meanSNRArray = rmi.GetMeanSNR(specFrames);
            double[] filteredMeanSNRArray = rmi.GetMeanSNR(filteredspecFrames);


            //for copy pastable data in GUI
            for (int spectral = 0; spectral < 32; spectral++) 
            {
                copyPasteBox.AppendText((spectral+1) + " Peak SNR: " + peakSNRArray[spectral].ToString() + " -> " + filteredPeakSNRArray[spectral].ToString());
                copyPasteBox.AppendText(Environment.NewLine);
            }
            for (int spectral = 0; spectral < 32; spectral++)
            {
                copyPasteBox.AppendText((spectral + 1) + " Mean SNR: " + meanSNRArray[spectral].ToString() + " -> " + filteredMeanSNRArray[spectral].ToString());
                copyPasteBox.AppendText(Environment.NewLine);
            }
            copyPasteBox.AppendText(testInfo);




            //setting array for X axis of channels on graphs
            double[] dataX = new double[32]; 
            for (int spectral = 0; spectral < 32; spectral++) 
            {
                dataX[spectral] = spectral + 1;
            }


            //FOR BASE IMAGES AND GRAPHS
            //snr graphs
            PSNR.Plot.AddScatter(dataX, peakSNRArray, color: Color.Blue, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "source SNR");
            var legend4 = PSNR.Plot.Legend(location: alignment);
            PSNR.Plot.Title(" Peak SNR for " + testName);
            PSNR.Plot.XLabel("Channel / -");
            PSNR.Plot.YLabel("Peak SNR / -");
            PSNR.Plot.YAxis.LabelStyle(fontSize: 24);
            PSNR.Plot.XAxis.LabelStyle(fontSize: 24);
            legend4.FontSize = fontSize;
            PSNR.Plot.Grid(false);
            PSNR.Plot.XAxis.TickMarkDirection(outward: false);
            PSNR.Plot.YAxis.TickMarkDirection(outward: false);

            MSNR.Plot.AddScatter(dataX, meanSNRArray, color: Color.Purple, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "source SNR");
            var legend3 = MSNR.Plot.Legend(location: alignment);
            MSNR.Plot.Title(" Mean SNR for " + testName);
            MSNR.Plot.XLabel("Channel / -");
            MSNR.Plot.YLabel("Mean SNR / -");
            MSNR.Plot.Grid(false);
            MSNR.Plot.XAxis.TickMarkDirection(outward: false);
            MSNR.Plot.YAxis.TickMarkDirection(outward: false);
            MSNR.Plot.YAxis.LabelStyle(fontSize: 24);
            MSNR.Plot.XAxis.LabelStyle(fontSize: 24);
            legend3.FontSize = fontSize;

            //range for colour bars
            double[] maxIntensity = rmi.GetMaxIntensity(filteredspecFrames);
            double[] minIntensity = rmi.GetMinIntensity(filteredspecFrames);

            
            double[] p1IntenseArray = rmi.GetIntensity(specFrames, Y1, X1);

            double[] p2IntenseArray = rmi.GetIntensity(specFrames, Y2, X2);
            double[] p3IntenseArray = rmi.GetIntensity(specFrames, Y3, X3);
            double[] p4IntenseArray = rmi.GetIntensity(specFrames, Y4, X4);

            double[] fp1IntenseArray = rmi.GetIntensity(filteredspecFrames, Y1, X1);
            for (int i = 0; i < fp1IntenseArray.Length; i++)
            {
                Debug.WriteLine(fp1IntenseArray[i]);
            }
            double[] fp2IntenseArray = rmi.GetIntensity(filteredspecFrames, Y2, X2);
            double[] fp3IntenseArray = rmi.GetIntensity(filteredspecFrames, Y3, X3);
            double[] fp4IntenseArray = rmi.GetIntensity(filteredspecFrames, Y4, X4);

            // intensity graphs
            //1
            Intensity1.Plot.AddScatter(dataX, p1IntenseArray, color: Color.Blue, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "Source pixel (" + X1 + ", " + (size-Y1) + ")"); //X1, size - Y1,
            Intensity1.Plot.AddScatter(dataX, fp1IntenseArray, color: Color.Blue, lineWidth: 2f, markerShape: MarkerShape.none, label: "Processed pixel (" + X1 + ", " + (size - Y1) + ")");
            
            Intensity1.Plot.AddScatter(dataX, p2IntenseArray, color: Color.Red, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "Source pixel (" + X2 + ", " + (size - Y2) + ")");
            Intensity1.Plot.AddScatter(dataX, fp2IntenseArray, color: Color.Red, lineWidth: 2f, markerShape: MarkerShape.none, label: "Processed pixel (" + X2 + ", " + (size - Y2) + ")");
            Intensity1.Plot.Title("Intensities for " + testName );
            Intensity1.Plot.XLabel("Channel / -"); 
            Intensity1.Plot.YLabel("Intensity / -"); 
            var legend = Intensity1.Plot.Legend(location: alignment);
            Intensity1.Plot.YAxis.LabelStyle(fontSize: 24);
            Intensity1.Plot.XAxis.LabelStyle(fontSize: 24);
            legend.FontSize = fontSize;


            Intensity1.Plot.Grid(false);
            Intensity1.Plot.XAxis.TickMarkDirection(outward: false);
            Intensity1.Plot.YAxis.TickMarkDirection(outward: false);
            Intensity1.Refresh();
            Intensity1.Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/Pixel_Intensities_for_" + testName + ".png"); //save

            //2

            Intensity2.Plot.AddScatter(dataX, p3IntenseArray, color: Color.Black, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "Source pixel (" + X3 + ", " + (size - Y3) + ")");
            Intensity2.Plot.AddScatter(dataX, fp3IntenseArray, color: Color.Black, lineWidth: 2f, markerShape: MarkerShape.none, label: "Processed pixel (" + X3 + ", " + (size - Y3) + ")");
            
            Intensity2.Plot.AddScatter(dataX, p4IntenseArray, color: Color.Green, markerShape: MarkerShape.none, lineStyle: LineStyle.Dash, label: "Source pixel (" + X4 + ", " + (size - Y4) + ")");
            Intensity2.Plot.AddScatter(dataX, fp4IntenseArray, color: Color.Green, lineWidth: 2f, markerShape: MarkerShape.none, label: "Processed pixel (" + X4 + ", " + (size - Y4) + ")");
            
            Intensity2.Plot.Title("Intensities for " + testName);
            Intensity2.Plot.XLabel("Channel / -");
            Intensity2.Plot.YLabel("Intensity / -"); 
            var legend2 = Intensity2.Plot.Legend(location: alignment);
            Intensity2.Plot.YAxis.LabelStyle(fontSize: 24);
            Intensity2.Plot.XAxis.LabelStyle(fontSize: 24);
            legend2.FontSize = fontSize;
            Intensity2.Plot.Grid(false);
            Intensity2.Plot.XAxis.TickMarkDirection(outward: false);
            Intensity2.Plot.YAxis.TickMarkDirection(outward: false);



            /////////////////////////////////////////////////////////////////////////////////////


            PSNR.Plot.AddScatter(dataX, filteredPeakSNRArray, lineWidth: 2f, markerShape: MarkerShape.none, color: Color.Blue, label: "post-filter PSNR");
            PSNR.Refresh();
            PSNR.Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/PeakSNR_for_[" + testName + "].png");//save

            MSNR.Plot.AddScatter(dataX, filteredMeanSNRArray, lineWidth: 2f, markerShape: MarkerShape.none, color: Color.Purple, label: "post-filter MSNR");
            MSNR.Refresh();
            MSNR.Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/MeanSNR_for_[" + testName + "].png");//save





            Intensity2.Refresh();
            Intensity2.Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/Pixel_Intensities_for_" + testName + ".png"); //save
            Intensity1.Refresh();
            Intensity1.Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/Pixel_Intensities_for_[" + testName + "].png"); //save







            for (int spectral = 0; spectral < 32; spectral++)  //save every channel
            {
                filteredspecFrames[spectral].SaveImage(projectDirectory + @"\savedimages\" + testName + "_" + filename + "_" + (spectral+1) + ".png");
            }
            FormsPlot[] imArray = new FormsPlot[3];
            imArray[0] = imPlot1;
            imArray[1] = imPlot2;
            imArray[2] = imPlot3;
            var hm = imPlot1.Plot.AddHeatmap(MatToDoubleArray(filteredspecFrames[7]), lockScales: false, colormap: Colormap.Grayscale ); // showing frames 8, 16, 24
            var hm2 = imPlot2.Plot.AddHeatmap(MatToDoubleArray(filteredspecFrames[15]), lockScales: false, colormap: Colormap.Grayscale) ;
            var hm3 = imPlot3.Plot.AddHeatmap(MatToDoubleArray(filteredspecFrames[23]), lockScales: false, colormap: Colormap.Grayscale);
            var cb = imPlot1.Plot.AddColorbar(hm);
            var cb1 = imPlot2.Plot.AddColorbar(hm2);
            var cb2 = imPlot3.Plot.AddColorbar(hm3);
            cb.MinValue = minIntensity[7];
            cb1.MinValue = minIntensity[15];
            cb2.MinValue = minIntensity[23];

            cb.MaxValue = maxIntensity[7];
            cb1.MaxValue = maxIntensity[15];
            cb2.MaxValue = maxIntensity[23];



            for (int i = 0; i < 3; i++)
            {
                imArray[i].Plot.Margins(0, 0);
                imArray[i].Plot.AddMarker(X1, size - Y1, MarkerShape.cross, 20, Color.Red);
                imArray[i].Plot.AddMarker(X2, size - Y2, MarkerShape.cross, 20, Color.Red);
                imArray[i].Plot.AddMarker(X3, size - Y3, MarkerShape.cross, 20, Color.Red);
                imArray[i].Plot.AddMarker(X4, size - Y4, MarkerShape.cross, 20, Color.Red);



                //var ch = imArray[i].Plot.AddCrosshair(X1, size-Y1);
                //var ch2 = imArray[i].Plot.AddCrosshair(X2, size-Y2);
                //var ch3 = imArray[i].Plot.AddCrosshair(X3, size-Y3);
                //var ch4 = imArray[i].Plot.AddCrosshair(X4, size - Y4);
                //string customFormatterX1(double position)
                //{
                //    return X1.ToString();
                //}
                //string customFormatterX2(double position)
                //{
                //    return X2.ToString();
                //}
                //string customFormatterX3(double position)
                //{
                //    return X3.ToString();
                //}
                //string customFormatterX4(double position)
                //{
                //    return X4.ToString();
                //}
                //string customFormatterY1(double position)
                //{
                //    return Y1.ToString();
                //}
                //string customFormatterY2(double position)
                //{
                //    return Y2.ToString();
                //}
                //string customFormatterY3(double position)
                //{
                //    return Y3.ToString();
                //}
                //string customFormatterY4(double position)
                //{
                //    return Y4.ToString();
                //}
                //ch.VerticalLine.PositionFormatter = customFormatterX1;
                //ch2.VerticalLine.PositionFormatter = customFormatterX2;
                //ch3.VerticalLine.PositionFormatter = customFormatterX3;
                //ch4.VerticalLine.PositionFormatter = customFormatterX4;
                //ch.HorizontalLine.PositionFormatter = customFormatterY1;
                //ch2.HorizontalLine.PositionFormatter = customFormatterY2;
                //ch3.HorizontalLine.PositionFormatter = customFormatterY3;
                //ch4.HorizontalLine.PositionFormatter = customFormatterY4;


                //imArray[i].Plot.YAxis.TickLabelNotation(invertSign: true);


                //ch.Color = System.Drawing.Color.Blue;
                //ch2.Color = System.Drawing.Color.Red;
                //ch3.Color = System.Drawing.Color.Black;
                //ch4.Color = System.Drawing.Color.Orange;



                imArray[i].Refresh();
                imArray[i].Plot.SaveFig(projectDirectory + "/ScottPlotGraphs/Graph_" + (i+1)*8 +  "_[" + testName + "].png");

            }

            
            
            
            //PSNR = max pixel value / standard deviation of signal in given area (background)

        }


        static Mat[] FilterChannels(Mat[] input, int ksize, double sX, double sY, bool spatial) //int kY, int kX, int sX, int sY
        {
            Mat[] cmspecFrames = new Mat[32];

            for (int Frame = 0; Frame < 32; Frame++)
            {
                cmspecFrames[Frame] = new Mat();
            }
            if (spatial)
            {
                //spatial filtering
                for (int spectral = 0; spectral < 32; spectral++)
                {
                    //Cv2.GaussianBlur(input[spectral], cmspecFrames[spectral], new OpenCvSharp.Size(ksize, ksize), sX, sX);
                    //Cv2.MedianBlur(input[spectral], cmspecFrames[spectral], ksize);
                    Cv2.BilateralFilter(input[spectral], cmspecFrames[spectral], ksize, sX, sY);

                }
            }
            else
            {
                //spectral filtering

                Mat[] spectralDir = new Mat[286 * 286];

                for (int Frame = 0; Frame < 32; Frame++)
                {
                    cmspecFrames[Frame] = new Mat("C:/Users/Archie/Documents/IPP/Image Processing Program/ImageProcessingProgram/blank.jpg", ImreadModes.Color);
                }
                for (int Frame = 0; Frame < 81796; Frame++)
                {
                    spectralDir[Frame] = new Mat("C:/Users/Archie/Documents/IPP/Image Processing Program/ImageProcessingProgram/black1d.jpg", ImreadModes.Color);
                }
                Mat[] filteredSpectralDir = spectralDir;


                int w = 0;


                for (int x = 0; x < 286; x++)
                {
                    for (int y = 0; y < 286; y++)
                    {

                        for (int i = 0; i < 32; i++)
                        {
                            var indexer = spectralDir[w].GetGenericIndexer<Vec3b>();
                            Vec3b color = new Vec3b();
                            color = input[i].Get<Vec3b>(x, y);
                            indexer[0, i] = color;

                        }
                        w++;


                    }
                }
                for (int Frame = 0; Frame < 81796; Frame++) //EDIT FILTER
                {
                    //Cv2.BilateralFilter(spectralDir[Frame], filteredSpectralDir[Frame], ksize, sX, sY);
                    //Cv2.MedianBlur(spectralDir[Frame], filteredSpectralDir[Frame], ksize);
                    Cv2.GaussianBlur(spectralDir[Frame], filteredSpectralDir[Frame], new OpenCvSharp.Size(ksize, ksize), sX);


                }
                w = 0;
                for (int y = 0; y < 286; y++)
                {
                    for (int x = 0; x < 286; x++)
                    {

                        for (int i = 0; i < 32; i++) // the channel
                        {
                            var indexer = cmspecFrames[i].GetGenericIndexer<Vec3b>();
                            Vec3b color = new Vec3b();
                            Vec3b oldColor = new Vec3b();
                            oldColor = spectralDir[w].Get<Vec3b>(0, i);
                            color = filteredSpectralDir[w].Get<Vec3b>(0, i);
                            indexer[y, x] = color;


                        }
                        w++;


                    }
                }
            }

            return cmspecFrames;


            

        }

        static void DisplayChannels(Mat[] source, Mat[] input, string name, bool isEdited)
        {
            double[] PSNRArray = new double[32];


            for (int spectral = 0; spectral < 32; spectral++) //displaying
            {


                //Cv2.ImShow("No. " + (spectral + 1) + " " + name, input[spectral]);



                
            }


        }
        static double[,] MatToDoubleArray(Mat source)
        {
            double[,] doubleArray = new double[286, 286];
            for (int y = 0; y < 286; y++)
            {
                for (int x = 0; x < 286; x++)
                {
                    Vec3b color = source.Get<Vec3b>(x, y);
                    doubleArray[x,y] = color.Item0;
                }
            }
            return doubleArray;
        }
        


        static void Display(Mat source, Mat input, string name, bool isEdited)
        {
            double snr = -1;

            if (isEdited)
            {
                try
                {
                    snr = Cv2.PSNR(source, input);
                }
                catch (Exception)
                {
                    snr = -2;
                }
            }
            new Window(name + " PSNR: " + snr, input);

        }

        private void FaveragePSNR_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void imPlot3_Load(object sender, EventArgs e)
        {

        }

        private void imPlot2_Load(object sender, EventArgs e)
        {

        }

        private void imPlot1_Load(object sender, EventArgs e)
        {

        }



        //// three methods of displaying image
        //new Window("source image", src);
        ////OpenCvSharp.Window.ShowImages(src);
        //Cv2.ImShow("source image", src);

        //Mat mBlurred = new Mat(); // median blur
        //Cv2.MedianBlur(src, mBlurred, 5);
        //Display(src, mBlurred, "Median Blurred");

        //Mat Canny = new Mat(); // edge detection algorithm
        //Cv2.Canny(src, Canny, 10, 192); //10, 192
        //Display(src, Canny, "Canny");

        //Mat biLat = new Mat(); // bilateral
        //Cv2.BilateralFilter(src, biLat, 5, 200, 200);
        //Display(src, biLat, "Bilateral");

        //Mat GBlur = new Mat();
        //Mat GBlur2 = new Mat();
        //Cv2.GaussianBlur(src, GBlur, new OpenCvSharp.Size(3, 3), 1.5, 0);
        //Cv2.GaussianBlur(src, GBlur2, new OpenCvSharp.Size(61, 61), 1.5, 0);
        //Display(src, GBlur, "Gaussian");
        //Display(src, GBlur2, "Gaussian 2");

        //Mat ColourMapped = new Mat();
        //Cv2.ApplyColorMap(src, ColourMapped, ColormapTypes.Jet);
        //Display(src, ColourMapped, "ColourMapped");

        //Cv2.Erode(input[spectral], cmspecFrames[spectral], new Mat()); // ?
        //Cv2.Dilate(input[spectral], cmspecFrames[spectral], new Mat()); // ?


        // BORDER TYPES (How do the denoisers approach the missing pixels of the kernel?)

        // BorderTypes.Constant - Adds a constant colored border (color optional)

        //BorderTypes.Wrap - repeats pixels to wrap

        //BorderTypes.Reflect - Mirror reflection of border Elements

        //BorderTypes.Reflect101 - Similar to reflect but doesnt include outer pixel. Reflect101 seems appropriate, matching given Powerpoint

        //BorderTypes.Replicate - takes border colour

        //BorderTypes.Default - Default is Reflect101


        //as edges may be useful, exploring use of Canny algorithm to highlight edges -rgb learning taken from https://blog.birost.com/a?ID=01050-0adc6c73-e76c-4a9d-bcdc-d6c78384e6b5

        //Mat Output = src;
        //for (int i = 0; i < Canny.Height; i++)
        //{
        //    for (int j = 0; j < Canny.Width; j++)
        //    {
        //        Vec3b color = Canny.Get<Vec3b>(i, j);//new Vec3b(); color channel type (byte triple), directly treated as Get generic method to return the specified type
        //        Vec3b black;
        //        black.Item0 = 0;
        //        black.Item1 = 0;
        //        black.Item2 = 0;
        //        //Get the specified channel pixels separately
        //        color.Item0 = Canny.Get<Vec3b>(i, j).Item0;//R
        //        color.Item1 = Canny.Get<Vec3b>(i, j).Item1;//G
        //        color.Item2 = Canny.Get<Vec3b>(i, j).Item2;//B

        //        if (color.Item0 + color.Item1 + color.Item2 > 1) //if the colour value is not black, add ontop of source image to show edges
        //        {
        //            bool seeWhite = false;
        //            if (seeWhite)
        //            {
        //                Output.Set<Vec3b>(i, j, color);
        //            }
        //            else
        //            {
        //                Output.Set<Vec3b>(i, j, black);
        //            }
        //        }


        //    }
        //}
        //Display(src, Output, "Noise on src");








    }
}