Vaatimusmäärittely 0.2 Blackjack  

TiVi22 

Aatu, Simo, Otto, Alex.	 

11.9.2023 
 
 

## 1. Johdanto 

1.1 Dokumentti sisältää Blackjackin vaatimusmäärittelyn. Dokumentissa on kuvattu tuotteen toiminnalliset vaatimukset, ei-toiminnalliset vaatimukset sekä dokumentin käyttötarkoitus ja menettely päivittämisen suhteen. Dokumenttia käytetään Blackjack –tuotteen toteuttamiseksi näiden yhdessä hyväksyttyjen vaatimusten mukaisesti. 

 _______
 
## 2. Toiminnalliset vaatimukset 
 
2.1 Toimiva blackjack peli. 

 

2.2 Mahdollisuus luoda ja kirjautua käyttäjälle. 

 
2.3 Rahasysteemi, pystyy asettaan panoksia ja pelaajalla on virtuaalinen saldo. 

 _______
 
## 3. Ei-toiminnalliset vaatimukset 
 

3.1 Käyttäjäkokemus: 

Helposti käytettävä ja houkutteleva käyttöliittymä.  

Sivustolla on helppo navigoida oikeisiin paikkoihin ja sisällön pitää olla selkeä. 

 

3.2 Suorituskyky. 

Nopea sivuston lataus ja toiminta ilman merkittäviä viiveitä. 

Laajennettavuus kasvavan käyttäjämäärän mukaisesti. 

 
 
3.3 Selainversio. 

Tuki useille suosituille selaimille kuten, Chrome, Firefox, Safari ja Edge. 

______
 
## 4. Tietokannan vaatimukset 

4.1 Tietokantaan tulee taulut Käyttäjät ja Rahat.

4.2 Käyttäjät tauluun tallentuu (id PRIMARY KEY, käyttäjänimi TEXT, salasana TEXT)

4.3 Rahat tauluun tallentuu (id PRIMARY KEY, käyttäjä_id, rahamäärä INTEGER)
 _______

## 5. Muut tiedot 

5.1 Tämä vaatimusdokumentti on luotu 4.9.2023. Netti Blackjack peliä varten, ja se on voimassa toistaiseksi. Dokumenttia päivitetään tarvittaessa sivuston kehittämiseksi. 
