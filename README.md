# Présentation

Projet pour le cours programmation Serveur.
L’association SportAsso souhaiterait avoir un site web pour gérer la préinscription/inscription et la gestion de ses adhérents (dossier complet, paiement).

# SiteWebMultiSport

Projet web multi-sports développé avec **ASP.NET Core 9.0**.

## Prérequis

Avant de lancer le projet, assurez-vous d'avoir installé les outils suivants :

- [**.NET 9.0 SDK**](https://dotnet.microsoft.com/download)
- [**Visual Studio 2022**](https://visualstudio.microsoft.com/downloads/)
- **LocalDB** (inclus avec SQL Server Express ou Visual Studio)

## Installation du projet

### 1. Cloner le projet

Clonez le projet en utilisant Git sur votre machine locale :

```bash
git clone https://github.com/votre-utilisateur/SiteWebMultiSport.git
cd SiteWebMultiSport
```

### 2. Configurer la base de données
Le projet utilise LocalDB par défaut. Si vous utilisez SQL Server, modifiez la chaîne de connexion dans le fichier appsettings.json :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SiteWebMultiSport;Trusted_Connection=True;"
}
```
3. Créer la base de données
Exécutez les commandes suivantes pour appliquer les migrations et initialiser la base de données :

```bash
dotnet ef migrations add InitialCreate   # Ajoute les migrations initiales
dotnet ef database update               # Applique les migrations et crée la base de données
```
Le DataSeeder sera exécuté automatiquement si la base de données est vide. Ce script insère des données de test pour les disciplines, sections, créneaux et adhérant.

4. Lancer le projet
Pour démarrer le projet en local, utilisez la commande suivante :

```bash
dotnet run
```
Le projet sera accessible sur https://localhost:5001 ou http://localhost:5000 par défaut.

### Utilisation
#### Créer un compte administrateur
Lors du premier lancement, un utilisateur administrateur avec le nom Admin et le mot de passe admin123 sera créé. Vous pouvez utiliser ces identifiants pour vous connecter et gérer les sections, disciplines, créneaux et autres éléments du projet.

#### Ajouter des sections et créneaux
En tant qu'administrateur, vous pouvez ajouter des disciplines, des sections et des créneaux depuis l'interface web. Les adhérents pourront ensuite s'inscrire à ces créneaux si leur abonnement est actif.

#### Inscription et gestion des adhérents
Les adhérents peuvent s'inscrire à des créneaux via l'interface utilisateur, sous réserve d'avoir un abonnement valide.
