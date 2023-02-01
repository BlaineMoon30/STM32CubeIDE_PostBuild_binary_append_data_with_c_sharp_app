# STM32CubeIDE_PostBuild_binary_append_data_with_c_sharp_app
STM32CubeIDE PostBuild binary append data_with c_sharp_app

This is an example of how to use Post-Build option when adding or merging the data to binary file build by STM32CubeIDE.

STM32CubeIDE -> Project Properties -> C/C++ Build -> Settings -> Build Steps -> Post-build steps -> Command -> "./PostBuild_BinFile_Append.exe"

New Feature
Added example of appending CRC32 or CRC16 to firmware binary after PostBuild.
