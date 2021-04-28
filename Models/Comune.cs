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
        public string NomeComune {get;set;}
        public string CodiceCatastale {get;set;}

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
            FileStream fout= new FileStream(fileName,FileMode.Create);
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

        public void Load()
        {
            string fn = NomeFile.Split(".")[0] + ".bin";
            Load(fn);
        }

        public void Load(string fileName)
        {
            //devo cancellare gli altri 8218 record per poi ricompilare con i nuovi
            this.Clear();

            FileStream fin = new FileStream(fileName,FileMode.Open);
            BinaryReader reader = new BinaryReader(fin);

            //fare un do while con ste robe
            Comune c = new Comune();
            //leggo l'ID
            c.ID= reader.ReadInt32();
            //leggo il CodiceCatastale
            c.CodiceCatastale= reader.ReadString();
            //leggo il Nome
            c.NomeComune= reader.ReadString();
            Add(c);

            //il modo + corretto di accorgersi della fine del file...?
            //manca un while che legge tutte le righe
            //come si fa ad accorgersi della fine del file?

        }
    }

}