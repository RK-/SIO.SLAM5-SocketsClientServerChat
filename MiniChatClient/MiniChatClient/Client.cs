using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MiniChatClient
{
    public class Client
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
	    {
		    TcpClient socketClient;
		    bool continuer = true;
            NetworkStream fluxReseau;
            StreamReader lecteurFlux;
            StreamWriter ecrivainFlux;
		
		    try
		    {
			    string IPServer = "127.0.0.1";
		        // créer le socket pour connexion (TcpClient) avec adresse IP du serveur et numéro de port 50000
                socketClient = new TcpClient(IPServer, 50000);
		        // afficher un message "Connecté au serveur " avec son adresse IP
                Console.WriteLine("Connecté au serveur " + IPServer);
                // récupérer un flux de données (NetWorkStream)
                fluxReseau = socketClient.GetStream();
		        // créer un lecteur de flux (StreamReader) et un écrivain de flux (StreamWriter)
                lecteurFlux = new StreamReader(fluxReseau);
                ecrivainFlux = new StreamWriter(fluxReseau);
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine("Connexion impossible\n" + e.ToString());
			    Console.ReadLine();
			    return;
		    }		
		    try
		    {
			    String messageDuClient = "";
			    String messageDuServeur = "";
			
			    while ( continuer )
			    {
				    // lire le message saisi par le client
                    Console.Write("Client : ");
                    messageDuClient = Console.ReadLine();
                    // envoyer le dernier message saisi au serveur
                    ecrivainFlux.WriteLine(messageDuClient);
                    ecrivainFlux.Flush();
				    // si le client a demandé une déconnexion en saisissant "bye" 
				    if (messageDuClient == "bye")
				    {
					    continuer = false;
				    }
				    else
				    {
					   // lire la réponse du serveur et l'afficher
                        messageDuServeur = lecteurFlux.ReadLine();
                        Console.WriteLine("Serveur : " + messageDuServeur);
				    } // fin du else
			    } // fin du while
		    } // fin du try
		    catch (Exception e)
		    {
			    Console.WriteLine("Problème réseau !\n"+ e.ToString());
			    Console.ReadLine();
		    }
			// fermer la connexion
            ecrivainFlux.Close();
            lecteurFlux.Close();
            fluxReseau.Close();
            socketClient.Close();
	    }
    }
}
