# ImageProcessingProgram
This program was designed to test different image processing filters on a set of relevant input images stored as a set of channels in text files describing pixel intensities.
The
# Spatial Filters
Spatial filters work across each channel indiviudally, taking a kernel size and filter intensity to smooth out the image channel through averaging of values in the kernel. This removes artifacts and harsh edges.
# Spectral Filters
Spectral filters not only consider a 2D kernel(for example 5 x 5 pixels) but work across channels to reduce inter-channel noise.

# Objective and Visual Tests
![image](https://github.com/dandertery/ImageProcessingProgram/assets/110602627/bcb4c544-c1ff-487a-951f-c67842ff6a95)

Output for each test consisted of:
  Raw SNR(Signal to Noise ratio) Data - useful to detect any anomalies during development or anomalies of the input channel image
  SNR Graphs comparing the image before and after processing - By comparing across all channels
  
