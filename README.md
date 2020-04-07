# AutoCar

The goal of this project is to automatically generate cars with machine learning based on the dataset provided.

## Dataset
The dataset used is https://github.com/nicolas-gervais/predicting-car-price-from-scraped-data/tree/master/picture-scraper
Please download the dataset and extract the images inside the AutoCar/Images folder. I did not include it in the repository because the dataset is ~690 MB in size.
The dataset also contains many images from a "real life" environment, meaning pictures on streets and so on. We only want pictures from cars that have a white background. To achieve this, please use the ImageSanitizerCLI.exe inside AutoCar\ImageSanitizer\bin. 
You need to specify the Images folder, so typically, you should start the program with 
```
./ImageSanitizer.exe ./../../Images
```
The deletion process takes a while and at the end of it you should have about 26.000 images left. Some of them might not fit our criteria exactly, however, the percentage should be very small.
