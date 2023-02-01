// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.IO;

Console.WriteLine("STM32CubeIDE_CRC");


string filePath = "";
string fileName = "";

string fileName_CRC_16 = "";
string fileName_CRC_32 = "";

string filePath_CRC_16 = "";
string filePath_CRC_32 = "";

filePath_CRC_16 = System.IO.Directory.GetCurrentDirectory();// Get the current path..
filePath_CRC_32 = System.IO.Directory.GetCurrentDirectory();


string[] binaryFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bin");
Console.WriteLine("Search for binary files in the current folder.");
foreach (string file in binaryFiles)
{
    Console.WriteLine(file);
    filePath = file;
    //fileName = System.IO.Path.GetFileName(filePath);// File name with extension.
    fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);// File name without extension.
}

Console.WriteLine("File Name is " + fileName);

using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    byte[] binaryData = new byte[fileStream.Length];
    fileStream.Read(binaryData, 0, (int)fileStream.Length);

    Console.WriteLine("Binary Total Size : " + binaryData.Length);

    /* CRC 16 */
    byte[] binaryData_Append_CRC_16 = (byte[])binaryData.Clone();
    Array.Resize(ref binaryData_Append_CRC_16, binaryData_Append_CRC_16.Length + 2);

    ushort crc_16 = 0xFFFF;
    for (int i = 0; i < binaryData.Length; i++)
    {
        crc_16 = (ushort)(crc_16 ^ (binaryData[i] << 8));
        for (int j = 0; j < 8; j++)
        {
            if ((crc_16 & 0x8000) == 0x8000)
            {
                crc_16 = (ushort)((crc_16 << 1) ^ 0x1021);
            }
            else
            {
                crc_16 = (ushort)(crc_16 << 1);
            }
        }
    }

    Console.WriteLine("CRC-16: 0x" + crc_16.ToString("X4"));
    binaryData_Append_CRC_16[binaryData_Append_CRC_16.Length - 2] = 0xFC;//BitConverter.GetBytes(crc_16)[0];
    binaryData_Append_CRC_16[binaryData_Append_CRC_16.Length - 1] = 0xFC;//BitConverter.GetBytes(crc_16)[1];


    /* CRC 16 Binary Padding .. */
    int alignment = 4;
    int remainder = binaryData_Append_CRC_16.Length % alignment;

    if (remainder != 0)
    {
        int padding = alignment - remainder;
        byte[] paddedArray = new byte[binaryData_Append_CRC_16.Length + padding];
        Array.Copy(binaryData_Append_CRC_16, paddedArray, binaryData_Append_CRC_16.Length);

        for (int i = binaryData_Append_CRC_16.Length; i < paddedArray.Length; i++)
        {
            paddedArray[i] = 0xff;
        }

        binaryData_Append_CRC_16 = paddedArray;
    }


    /* CRC 16 Generate the new file name */
    fileName_CRC_16 = fileName + "CRC_16.bin";
    Console.WriteLine("CRC_16 Newfile Name is " + fileName_CRC_16);

    /* CRC 16 Create Final Binary */
    string filePath_CRC_16_Full = filePath_CRC_16 + fileName_CRC_16;
    Console.WriteLine("CRC_16 file path is " + filePath_CRC_16_Full);

    using (System.IO.FileStream fs = System.IO.File.Create(filePath_CRC_16_Full))
    {
        fs.Write(binaryData_Append_CRC_16, 0, binaryData_Append_CRC_16.Length);
    }



    /* CRC 32 */
    byte[] binaryData_Append_CRC_32 = (byte[])binaryData.Clone();
    Array.Resize(ref binaryData_Append_CRC_32, binaryData_Append_CRC_32.Length + 4);

    uint crc_32 = 0xFFFFFFFF;
    for (int i = 0; i < binaryData.Length; i++)
    {
        crc_32 = crc_32 ^ binaryData[i];
        for (int j = 0; j < 8; j++)
        {
            if ((crc_32 & 1) == 1)
            {
                crc_32 = (crc_32 >> 1) ^ 0xEDB88320;
            }
            else
            {
                crc_32 = crc_32 >> 1;
            }
        }
    }

    crc_32 = crc_32 ^ 0xFFFFFFFF;

    Console.WriteLine("CRC-32: 0x" + crc_32.ToString("X8"));
    binaryData_Append_CRC_32[binaryData_Append_CRC_32.Length - 4] = BitConverter.GetBytes(crc_32)[0];
    binaryData_Append_CRC_32[binaryData_Append_CRC_32.Length - 3] = BitConverter.GetBytes(crc_32)[1];
    binaryData_Append_CRC_32[binaryData_Append_CRC_32.Length - 2] = BitConverter.GetBytes(crc_32)[0];
    binaryData_Append_CRC_32[binaryData_Append_CRC_32.Length - 1] = BitConverter.GetBytes(crc_32)[1];

    /* CRC 32 Generate the new file name */
    fileName_CRC_32 = fileName + "CRC_32.bin";
    Console.WriteLine("CRC_32 Newfile Name is " + fileName_CRC_32);

    /* CRC 16 Create Final Binary */
    string filePath_CRC_32_Full = filePath_CRC_32 + fileName_CRC_32;
    Console.WriteLine("CRC_32 file path is " + filePath_CRC_32_Full);

    using (System.IO.FileStream fs = System.IO.File.Create(filePath_CRC_32_Full))
    {
        fs.Write(binaryData_Append_CRC_32, 0, binaryData_Append_CRC_32.Length);
    }



    /* Program Finished */
    Console.WriteLine("Program finished !!");
}