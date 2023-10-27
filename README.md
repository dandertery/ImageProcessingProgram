# ImageProcessingProgram
This program was designed to test different image processing filters on a set of relevant input images
The input images were  stored as a set of channels in text files describing pixel intensities. Metadata at the top of each file was removed. The pixel intensities were then normalised from 0-255
and the intensities reformatted into a set of z (channels of) x*y (image) matrices to prepare for kernel filtering.
# Spatial Filters
Spatial filters work across each channel individually, taking a kernel size and filter intensity to smooth out the image channel through averaging of values in the kernel. This removes artifacts and harsh edges.
Filters tested spatially:
  Median Blur  
  Gaussian Blur  
  Bilateral Filter  
  Erode / Dilate (not found to be useful)

# Spectral Filters
Spectral filters not only consider a 2D kernel(for example 5 x 5 pixels) but work across channels to reduce inter-channel noise. This is achieved by filtering
a kernel consisting of a pixel and its corresponding pixels in the other channels. This kernel size is therefore (1x1xn) depending on n channels.
Filters tested spectrally:  
  Median Blur  
  Gaussian Blur  
# Objective and Visual Tests


Output for each test consisted of:
  Raw SNR(Signal to Noise ratio) Data - useful to detect any anomalies during development or anomalies of the input channel image  
  SNR Graphs comparing the image before and after processing - By filtering across all channels inter-channel noise can be reduced.  
  Pixel Intensity Graph comparing before and after. If pixel intensities drastically reduce across several channels the filter is likely very lossy
  
  Note: Processed input images and some text have been censored with red  
  
  ![image](https://github.com/dandertery/ImageProcessingProgram/assets/110602627/bcb4c544-c1ff-487a-951f-c67842ff6a95)
