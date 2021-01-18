# ProxyServer

Studentnaam: Daan Bakker  
Studentnummer: 601945

# Algemene beschrijving applicatie

Een .NET C# WPF applicatie voor de course NOTS WIN.
In deze applicatie is een proxy server ontwikkeld op basis van het TCP-Protocol. Die het HTTP protocol na bootst.

![Proxy_server](https://static.packt-cdn.com/products/9781789532975/graphics/de402723-a3f2-4d4b-b290-563b7cdbc32d.png)


##  Ontwerp architectuur
Ontwerp en bouw de *architectuur* van de applicatie die HTTP-requests van een willekeurige PC opvangt en doorstuurt naar één webserver. 
(Teken een diagram en licht de onderdelen toe)


##  Voorbeeld van een http-request en van een http-response. 
(Kan je globale overeenkomsten vinden tussen een request en een response?)  (Teken een diagram en licht de onderdelen toe)


##  TCP/IP
###  Beschrijving van concept in eigen woorden
#### TCP - TCP staat voor Transmission Control Protocol. In het nederlands Overdracht controle protocol.
#### IP - IP staat voor Internet protocol. Een protocol wat dus bedoeld is voor over het internet.

Een protocol is een soort verzameling van afspraken waar gebruikers zich aan dienen te houden.

#### Lagen
Het TCP/IP samen is dus een overdracht protocol voor het internet. Dit protocol bestaat uit verschillende lagen:
- Applicaite laag - Deze laag word gebruikt door programma's zoals een browser gebruikt om mee te communiceren. Een bekend protocol van deze laag is het HTTP 
- Transport laag - In deze laag word gebruik gemaakt van TCP en UDP. TCP en UDP pakken de data die moet worden verzonden en maken hier kleine pakketjes van. UDP is sneller maar dit heeft zijn risico's zoals het verliezen van verzonden pakketjes. Dit word vaak gebruikt voor video bellen. (Een verloren pakketje bijvoorbeeld wanneer de camera van je gesprekspartner tijdelijk blijft hangen). TCP doet dit niet deze stuurt een pakket opnieuw wanneer deze niet aan komt. Hierdoor blijft het verzonden bericht zoals deze is bedoeld. Om ervoor te zorgen dat duidelijk is welke pakketjes er bij elkaar horen worden deze samen verstuurt met een Header.
- Internet laag - Over deze laag worden de pakketjes uit de transport laag verstuurt naar het uitiendelijke adres waar deze voor bedoelt zijn. Dit word gedaan aan de hand van het IP protocol.
- Netwerk laag - Op deze laag worden de pakketjes verstuurt naar het juiste apparaat die de boodschap moet ontvangen.

###  Code voorbeeld van je eigen code
###  Alternatieven & adviezen
###  Authentieke en gezaghebbende bronnen


##  Bestudeer de RFC van HTTP 1.1.
###  Hoe ziet de globale opbouw van een HTTP bericht er uit? (Teken een diagram en licht de onderdelen toe)
###  Uit welke componenten bestaan een HTTP bericht.  (Teken een diagram en licht de onderdelen toe)
###  Hoe wordt de content in een bericht verpakt? (Teken een diagram en licht de onderdelen toe)
###  Streaming content 

##  Kritische reflectie op eigen werk (optioneel, maar wel voor een 10)
###  Wat kan er beter? Geef aan waarom?
###  Wat zou je een volgende keer anders doen?
###  Hoe zou de opdracht anders moeten zijn om er meer van te leren?

# Test cases

### Case naam
### Case handeling
### Case verwacht gedrag

# Kritische reflectie op eigen beroepsproduct

### Definieer kwaliteit in je architectuur, design, implementatie. 
### Geef voorbeelden.
### Wat kan er beter, waarom? 
