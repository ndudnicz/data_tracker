const sqlite3 = require('sqlite3').verbose();

// Créer ou ouvrir la base de données
const db = new sqlite3.Database('data.db', (err) => {
    if (err) {
        console.error('Erreur lors de l\'ouverture de la base de données:', err.message);
    } else {
        console.log('Base de données SQLite créée ou ouverte avec succès.');
    }
});

// Créer la table data
db.serialize(() => {
    db.run(`
        CREATE TABLE IF NOT EXISTS data (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            value REAL NOT NULL,
            created_at TEXT NOT NULL
        )
    `, (err) => {
        if (err) {
            console.error('Erreur lors de la création de la table:', err.message);
        } else {
            console.log('Table "data" créée avec succès.');
        }
    });
});

// Fermer la connexion proprement
db.close((err) => {
    if (err) {
        console.error('Erreur lors de la fermeture de la base de données:', err.message);
    } else {
        console.log('Connexion à la base de données fermée.');
    }
});
