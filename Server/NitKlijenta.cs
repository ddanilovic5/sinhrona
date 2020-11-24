using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domen;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server
{
	class NitKlijenta
	{
		private NetworkStream tok;
		BinaryFormatter formater;
		public NitKlijenta(NetworkStream tok)
		{
			this.tok = tok;
			formater = new BinaryFormatter();

			ThreadStart ts = Obradi;
			new Thread(ts).Start();
		}

		void Obradi()
		{
			try
			{
				int operacija = 0;
				while (operacija != (int)Operacije.Kraj)
				{
					TransferKlasa transfer = formater.Deserialize(tok) as TransferKlasa;

					switch (transfer.Operacije)
					{
						case Operacije.SlanjePoruke:
							transfer.Odgovor = $"Ovo je odgovor za {transfer.korisnik.Username}, a ovo je poruka : {transfer.Poruka}";
							formater.Serialize(tok, transfer);
							break;
						case Operacije.Kraj:
							operacija = 1;
							break;
						default:
							break;
					}
				}
			}
			catch (Exception)
			{
				
			}
		}
	}
}
