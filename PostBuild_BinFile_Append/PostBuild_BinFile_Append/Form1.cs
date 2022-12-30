namespace PostBuild_BinFile_Append
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        static void WriteBinayData()
        {
            byte[] arrData = { 'M', 'A', 'G', 'I', 'C' };

            //Stream stream = new FileStream("stm32f413_nucelo_post_build_append_bin.bin", FileMode.OpenOrCreate);
            Stream stream = new FileStream("stm32f413_nucelo_post_build_append_bin.bin", FileMode.Append);
            using (BinaryWriter wr = new BinaryWriter(stream))
            {
                wr.Write(arrData);
            }
        }

        static void ReadBinaryData()
        {
            using (BinaryReader br = new BinaryReader(File.Open("stm32f413_nucelo_post_build_append_bin.bin", FileMode.Open)))
            {
                int a;
                string b;
                byte[] arrData = new byte[3];

                a = br.ReadInt32();
                b = br.ReadString();
                arrData = br.ReadBytes(3);

                Console.WriteLine("a:{0}", a);
                Console.WriteLine("b:{0}", b);
                for (int i = 0; i < arrData.Length; i++)
                {
                    Console.WriteLine("c:{0:x2}", arrData[i]);
                }
            }
        }

        static void ReadBinaryData_Per_Bytes()
        {
            using (BinaryReader br = new BinaryReader(File.Open("stm32f413_nucelo_post_build_append_bin.bin", FileMode.Open)))
            {
                long dataLength = br.BaseStream.Length;
                byte[] readData = new byte[br.BaseStream.Length];
                readData = br.ReadBytes((int)br.BaseStream.Length);

                for (int i = 0; i < readData.Length; i++)
                {
                    Console.WriteLine("Read_Data_From_Bin{0}:{1:x2}", i, readData[i]);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadBinaryData_Per_Bytes();
            WriteBinayData();
        }
    }
}