using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace MiniChatServeur
{
    public class Serveur
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
           try
           {
	        bool continuer = true;
	        String messageDuClient = "";
	        String messageDuServeur = "";				
	        IPAddress adresseIP = IPAddress.Parse("127.0.0.1");

            // créer un socket d'écoute (un écouteur = socketEcouteur) avec adresse IP et numéro de port 50000
            TcpListener socketEcouteur = new TcpListener(adresseIP, 50000);
	        // démarrer l'écoute
            socketEcouteur.Start();
            Console.WriteLine("Serveur démarré...");
	        // extraire une connexion pour client(classe TcpClient) reste bloqué ici tant que le client n'est pas connecté
            TcpClient socketServiceClient = socketEcouteur.AcceptTcpClient();
	        // afficher "Un client vient de se connecter "  
            Console.WriteLine("Un client vient de se connecter.\n");
	        // créer un flux de données réseau (networkStream), 
            NetworkStream fluxReseau = socketServiceClient.GetStream();
            // créer un lecteur de flux (StreamReader) 
            StreamReader lecteurFlux = new StreamReader(fluxReseau);
            // créer un écrivain de flux (StreamWriter)
            StreamWriter ecrivainFlux = new StreamWriter(fluxReseau);
	        // tant que le client ne met pas fin à la connexion
	        while (continuer)
	        {
		        if (socketServiceClient.Connected)
		        {
			        // lire et afficher les données envoyées par le client
                    messageDuClient = lecteurFlux.ReadLine();
                    Console.WriteLine("Client : " + messageDuClient);
			        // si le client se déconnecte en saisissant "bye"
			        if (messageDuClient == "bye")
			        {   
                        // arrêter l’écoute 	
                        continuer = false;
                    }
                    else
			        {  
                        // lire le message saisi côté serveur
                        Console.Write("Serveur : ");
                        messageDuServeur = Console.ReadLine();
			            // envoyer le message saisi au client  
                        ecrivainFlux.WriteLine(messageDuServeur);
                        ecrivainFlux.Flush(); // Forcer l'envoi du message au client
                    }
		        }
	        } // fin du while
	        // fermer la connexion
            ecrivainFlux.Close();
            lecteurFlux.Close();
            fluxReseau.Close();
            socketServiceClient.Close();
	        //fermer l'écouteur
            socketEcouteur.Stop();
            Console.WriteLine("Arrêt du serveur");
            Console.ReadLine();
           }
           catch (Exception e )
           {
   	        Console.WriteLine(e.ToString());
            Console.ReadLine();
           }
        }// fin du Main
    }// fin de la classe
}