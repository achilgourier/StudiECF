# StudiECF

French Game - Site Web sur les Jeux Vidéo

French Game est une application ASP.NET dédiée aux passionnés de jeux vidéo francophones. Cette plateforme offre un espace interactif pour discuter, partager et explorer l'univers vaste et diversifié des jeux vidéo.


Prérequis

Assurez-vous d'avoir installé les éléments suivants avant de lancer l'application :

    ASP.NET Core
    Visual Studio (ou tout autre éditeur de texte/code de votre choix)

Migrations de Base de Données

Avant de lancer l'application pour la première fois, assurez-vous de mettre à jour la base de données en exécutant les migrations. Ouvrez une console dans le répertoire de votre projet et exécutez les commandes suivantes :

dotnet ef migrations add InitialCreate
dotnet ef database update

Ces commandes utiliseront Entity Framework Core pour créer la base de données en fonction de vos modèles.
Lancement de l'Application

    Ouvrez le projet dans Visual Studio ou utilisez la ligne de commande.

    Assurez-vous d'avoir sélectionné le projet principal comme projet de démarrage.

    Appuyez sur F5 ou exécutez dotnet run dans la console.

L'application devrait être accessible à l'adresse https://efcfrenchgame.azurewebsites.net/.
