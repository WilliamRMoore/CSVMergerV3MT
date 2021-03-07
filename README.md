# CVSMergerV3MT

# What thsi program does

This program merges CSVs based on user input in the form of mapping rules.
Say you have two CSVs that contain similar columns, maybe you have two sets that contain a column for temperature (that is recorded in the same metric, such as culsius). If you know the names of those columns, you can map each of those columns from your two data sets into a single column of a new data set.

This program also utilizes Multi Threading. This is important if you have particularly large data sets, say over a gb in size. This program can proccess large files very quickly by utilizing multiple threads on your machine (you can specify how many). It also doesn't read the entire file into memory, but instead only works with chunks at time, so as to avoid memory utilization issues.

I wrote this program as an effort to help extract multiple identical columns from disperate CSVs with differing schemas. 

# How to run

Clone this repo.

Ensure you have the DOTNET 3.1 runtime installed on your machine

Go to the folder that contains the program entry point

Open a command line and use dotnet run.

# How to use

You will need atleast one CSV data set, this is fine if all you want to do is extract specific columns. But it will work with multiple data sets if you want to merge.

Run the program, it will prompt you to enter the name of your new data set.
It will then ask you to list the names of the columns you want in the new dataset (You'll need to have knowledge of the data set(s) you are merging so that can know wich columns will fit into your new schema)

It will then ask you for a destination folder for your new output set. 

It will then ask you for the file path of the input set(s) (the sets you want to merge or extract columns from). 

From here an input loop will start, and it will iterate through the columns of the input sets asking which column you want to map to the output set. 

Once you have mapped your sets it will then ask if you want to use multiple threads for proccessing (it will list the maximum number of threads your machine has available, but it uses a minum of three, one for reading, one for processing, and one for writting).

A prompt will then show you the configuration for your new set with all of the mapping rules that you input. You must either accept or reject this output before proceeding. 

Once accepted, it will prepare the job, and show you the total number of output lines.

The next prompt says hit any key to run, but that's a lie. You have to hit enter, I'm going to fix this.

While the job executes you will see a precentage counter for the job as a visualization for it's progress. 

Once done it will tell you how long the job took. 

Your newly made CSV should be in the directory you specified. 


# Known Issues

1. The regex used for splitting CSV lines, while robust, isn't perfect. I have encountered files with formatting that will cause the program to crash with an "index out of bounds error". I have only seen this with files that were exported with older version of Excel, though.
2. The input loop for the column mapping rules has an option to go back, it doesn't work properly, and I probably won't fix it for this version of the project. This will be addressed when I upgrade the project to have a GUI. 

