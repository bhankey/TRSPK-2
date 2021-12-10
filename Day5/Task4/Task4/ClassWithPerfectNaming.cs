using System;
using System.IO;

namespace Task4
{
    public class ClassWithPerfectNaming: IDisposable
    {
        private string F;
        private string S;

        private FileStream FStream;
        private FileStream SStream;
        
        private bool disposed = false;
        public ClassWithPerfectNaming(string f, string s)
        {
            (F, S) = (f, s);
        }

        public void Open()
        {
            try
            {
                FStream = File.OpenRead(F);
                SStream = File.OpenWrite(S);
            }
            catch (Exception e)
            {
                FStream?.Close();
                SStream?.Close();
                throw;
            }
        }

        public void Work()
        {
            using (var streamReader = new StreamReader(FStream))
            using (var streamWriter = new StreamWriter(SStream))
            {
                while (true)
                {
                    var line = streamReader?.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    
                    streamWriter.WriteLine(line);
                }
            }
        }

        public void Close()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    F = "";
                    S = "";
                }
                FStream?.Dispose();
                SStream?.Dispose();
                
                disposed = true;
            }
        }
        
        ~ClassWithPerfectNaming()
        {
            Dispose(false);
        }
    }
}