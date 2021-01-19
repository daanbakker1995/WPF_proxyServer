# ProxyServer

Studentnaam: Daan Bakker  
Studentnummer: 601945

# Algemene beschrijving applicatie

Een .NET C# WPF applicatie voor de course NOTS WIN.
In deze applicatie is een proxy server ontwikkeld op basis van het TCP-Protocol. Die het HTTP protocol nabootst.

![Proxy_server](https://static.packt-cdn.com/products/9781789532975/graphics/de402723-a3f2-4d4b-b290-563b7cdbc32d.png)


##  Ontwerp architectuur
Ontwerp en bouw de *architectuur* van de applicatie die HTTP-requests van een willekeurige PC opvangt en doorstuurt naar een webserver. 
(Teken een diagram en licht de onderdelen toe)

##  Voorbeeld van een http-request en van een http-response. 
(Kan je globale overeenkomsten vinden tussen een request en een response?)  (Teken een diagram en licht de onderdelen toe)


##  TCP/IP
###  Beschrijving van concept in eigen woorden
#### TCP staat voor Transmission Control Protocol. Dit is een Overdracht controle protocol.
#### IP staat voor Internet protocol. Een protocol wat dus bedoeld is voor over het internet.

> Een protocol is een soort verzameling van afspraken waar gebruikers zich aan dienen te houden.

#### Lagen
Het TCP/IP samen is dus een overdracht protocol voor het internet. Dit protocol bestaat uit verschillende lagen:
- Applicatie laag - Deze laag wordt gebruikt door programma's zoals een browser gebruikt om mee te communiceren. Een bekend protocol van deze laag is het Hypertext Transfer Protocol (HTTP).
- Transport laag - In deze laag wordt gebruik gemaakt van TCP en UDP. TCP en UDP pakken de data die moet worden verzonden en maken hier kleine pakketjes van. UDP is sneller maar dit heeft zijn risico's zoals het verliezen van verzonden pakketjes. Dit wordt vaak gebruikt voor video bellen. (Een verloren pakketje bijvoorbeeld wanneer de camera van je gesprekspartner tijdelijk blijft hangen). TCP doet dit niet deze stuurt een pakket opnieuw wanneer deze niet aan komt. Hierdoor blijft het verzonden bericht zoals deze is bedoeld. Om ervoor te zorgen dat duidelijk is welke pakketjes er bij elkaar horen worden deze samen verstuurt met een Header.
- Internet laag - Over deze laag worden de pakketjes uit de transportlaag verstuurt naar het uiteindelijk adres waar deze voor bedoeld zijn. Dit wordt gedaan aan de hand van het IP protocol.
- Netwerk laag - Op deze laag worden de pakketjes verstuurt naar het juiste apparaat die de boodschap moet ontvangen.

###  Code voorbeeld van je eigen code
Hieronder is een stuk code uit de multichat app. In deze code is te zien hoe er data wordt ontvangen aan de hand van een networkstream.
Deze data wordt als bytes ontvangen en vervolgens met een stringbuilder samengevoegd tot een bericht. Deze chat app maakt gebruik van een endOfTransmissionCharacter (een karakter om het einde van het bericht aan te geven). Hierna wordt dit karakter van het bericht af gehaald en toegevoegd aan UI. Omdat deze code zich op de server bevindt, wordt het bericht gebroadcast naar de aangesloten clients van de chat. 
```
while (networkStream != null && networkStream.CanRead)
{
    // Receive data from stream
    byte[] byteArray = new byte[BUFFERSIZE];
    int resultSize = networkStream.Read(byteArray, 0, BUFFERSIZE);
    string message = Encoding.ASCII.GetString(byteArray, 0, resultSize);

    // Make one message from received bytes
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(message);

    //end of Message
    if (message.EndsWith(EndOfTransitionCharacter))
    {
        // Make message readable
        string clientMessage = stringBuilder.ToString(); 
        clientMessage = clientMessage.Remove(clientMessage.Length - EndOfTransitionCharacter.Length);
        if (clientMessage == "bye")
            break;
        // Display message in chat
        AddMessageToChat(clientMessage);
        // Send message to other connected clients
        BroadCast(clientMessage, tcpClient);
        // Empty stringBuilder for new message
        stringBuilder = new StringBuilder();
    }
} 
```

###  Alternatieven & adviezen
Een alternatief voor het gebruik van TCP is UDP. UDP is zoals al eerder benoemd niet geschikt wanneer pakketjes in zijn geheel een boodschap vormen. Zoals in een chat applicatie. UDP is wel uitstekend voor video bellen omdat het veel sneller is dan TCP. Ook voor spellen zou dit geen probleem moeten zijn wanneer je een online multiplayer spel ontwikkeld. Hierdoor hebben de gebruikers snel een reactie van wat de andere speler doet. 
###  Authentieke en gezaghebbende bronnen
- [IETF Tools over TCP](https://tools.ietf.org/html/rfc793)
- [Docs microsoft - NetworkStream](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream?view=net-5.0)


##  Bestudeer de RFC van HTTP 1.1.
###  Hoe ziet de globale opbouw van een HTTP bericht er uit? (Teken een diagram en licht de onderdelen toe)
Een HTTP Message bevat het volgende: 

- Een startline dit kan een request line zijn of een status line.
Request line
`GET /HTTP/1.1`
Status line
`HTTP/1.1 200 OK`
- Verder bevat een message overige headers.
- Hierna een lege lijn. Dit geeft het einde van de headers aan.
- Onder deze lijn komt de Body. Dit kan bijvoorbeeld een HTML bestand, afbeelding of video zijn.

Hieronder een voorbeeld van een request en een response:

![HTTP Message](http.png)

# Test cases

### Case naam
### Case handeling
### Case verwacht gedrag

| Case Naam | Handeling |  Verwacht gedrag |
|----------|----------|------|
| 1.Tekst in ip adres |  voer tekst in het veld in | foutmelding server start niet |
| 2.Tekst in port nummer | voer tekst in het veld in | foutmelding server start niet |
| 3.Tekst in buffergrootte | voer tekst in het veld in | foutmelding server start niet |
| 4.Geen ipadres in ipadres veld |  voer '12345' in het veld in | foutmelding server start niet |
| 5.Geen waardes |  voer niets in invoervelden in | foutmelding server start niet |



# Kritische reflectie op eigen beroepsproduct

### Definieer kwaliteit in je architectuur, design, implementatie. 
### Geef voorbeelden.
### Wat kan er beter, waarom? 
