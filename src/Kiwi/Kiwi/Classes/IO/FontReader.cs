using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwi.Classes {

    internal class FontReader : BinaryReader {

        public long Length { 
            get => BaseStream.Length; 
        }
        public long Position { 
            get => BaseStream.Position; 
        }

        public FontReader(Stream stream) : base(stream, Encoding.UTF8) {

        }

        public void Move(int offset) {
            BaseStream.Position = offset;
        }

        public string ReadString(int offset) {
            try {

                Move(offset);

                var count = 0;
                while (ReadByte() != 0x00)
                    count++;

                Move(offset);
                return (count >= 1)
                    ? Encoding.UTF8.GetString(ReadBytes(count))
                    : "";

            }
            catch {
                return "";
            }
        }
        public override string ReadString() {
            try {

                var count = 0;
                while (PeekChar() != 0x00) {
                    ReadByte();
                    count++;
                }

                Move((int)Position - count);
                return (count >= 1)
                    ? Encoding.UTF8.GetString(ReadBytes(count))
                    : "";

            }
            catch {
                return "";
            }
        }

    }

}