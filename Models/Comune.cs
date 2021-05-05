using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography;
namespace Bertozzi.Mattia._4H.SaveRecord.Models
{
    public class Comune
    {
        public int ID {get;set;}

        private string _nomeComune;
        public string NomeComune 
        {
            get=>_nomeComune;
            set
            {
                /*
                Lunghezza record = 32
                ID = 4 Byte
                CodiceCatastale=4+1
                NomeComune == 22+1
                */
                if(value.Length==22)
                {
                    _nomeComune=value;
                }
                if(value.Length<22)
                    value = value.PadRight(22);
                else if(value.Length>22)
                    value = value.Substring(0,22);

                _nomeComune=value;
                
            }
        }
        private string _codiceCatastale;
        public string CodiceCatastale 
        {
            get=>_codiceCatastale;

            set
            {
                if(value.Length==4)
                {
                    _codiceCatastale=value;
                }
                if(value.Length<4)
                    value = value.PadRight(4);
                else if(value.Length>4)
                    value = value.Substring(0,4);

                _codiceCatastale=value;
            }
        }

        public Comune(){}

        //readline è un contatore
        public Comune(string stringa,int id)
        {
            //creo l'oggetto comune partendo dalla stringa csv
            string[] colonne = stringa.Split(',');

            ID=id;

            CodiceCatastale = colonne[0];
            NomeComune = colonne[1];
            
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID: {ID}");
            sb.AppendLine($"Codice Catastale: {CodiceCatastale}");
            sb.AppendLine($"NomeComune: {NomeComune}");
            return sb.ToString();
        }
    }
    public class Comuni : List<Comune> //Comuni è un List<Comune>
    {
        public string NomeFile {get;}
        public Comuni(){}

        public Comuni(string fileName)
        {
            NomeFile = fileName;

            using (FileStream fin = new FileStream(fileName,FileMode.Open))
            {
                StreamReader reader = new StreamReader(fin);
                int id=1;

                while(!reader.EndOfStream)
                {
                    string riga = reader.ReadLine();
                    Comune c = new Comune(riga, id);
                    //this è la List<Comune>
                    //this.Add(c);//this si può omettere
                    Add(c);

                    id++;
                }
            }
        }
        public void Save()
        {
            // string[] colonne = NomeFile.Split(".");
            // string fileName = colonne[0];

            string fn = NomeFile.Split(".")[0] + ".bin";
            Save(fn);
        }
        
        public void Save(string fileName)
        {
            using(FileStream fout= new FileStream(fileName,FileMode.Create))
            {
               BinaryWriter writer = new BinaryWriter(fout);

                foreach(Comune comune in this)
                {
                writer.Write(comune.ID);
                writer.Write(comune.CodiceCatastale);
                writer.Write(comune.NomeComune);
                }
                writer.Flush();
                writer.Close(); 
            }             
        }

        public void Load()
        {
            string fn = NomeFile.Split(".")[0] + ".bin";
            Load(fn);
        }

        public void Load(string fileName)
        {
            //devo cancellare gli altri 8218 record per poi ricompilare con i nuovi
            this.Clear();

            using(FileStream fin= new FileStream(fileName,FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fin);

                //fare un do while con ste robe
                Comune c = new Comune();

                while(reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    c.ID= reader.ReadInt32();
                    //leggo il CodiceCatastale
                    c.CodiceCatastale= reader.ReadString();
                    //leggo il Nome
                    c.NomeComune= reader.ReadString();
                    Add(c);
                }

            }

            //il modo + corretto di accorgersi della fine del file...?
            //manca un while che legge tutte le righe
            //come si fa ad accorgersi della fine del file?

        }

        public Comune RicercaComune(int numero)
        {
            FileStream fin = new FileStream("Comuni.bin",FileMode.Open);
            BinaryReader reader = new BinaryReader(fin);
            
            fin.Seek((numero-1)*32,SeekOrigin.Begin);
            Comune c = new Comune();
            c.ID = reader.ReadInt32();
            c.CodiceCatastale= reader.ReadString();
            c.NomeComune= reader.ReadString();


            return c;
        }
    }

}