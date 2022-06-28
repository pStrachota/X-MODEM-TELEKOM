<div id="top"></div>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">


<h3 align="center">X-MODEM (WITH CRC-16 CHECKSUM EXTENSION) TELEKOM PROJECT</h3>

  <p align="center">
    C# GUI project for telecommunication class in TUL.
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About the project</a></li>
    <li><a href="#built-with">Built With</a></li>
    <li><a href="#some-screens-from-project">Some screens from project</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
One of the tasks made for telecomunnication class in TUL. Other is [AD/CA audio converter](https://github.com/pStrachota/AC-CA-AUDIO).
We had to implement X-MODEM protocol, using algebraic and CRC-16 checksum. GUI was not necessary but in my opionion, this solution
is much easier for user to choose file to send. As it can be seen below, beside checksum, also specific baud rate can be used.
This program not only enable X-MODEM comunnication is one instance of the program, but also between multiple instances and 
even with [hyperterminal](https://en.wikipedia.org/wiki/HyperACCESS).

If specific ports are not available, virtual emulators can be used - during demonstration of my code I'm using free version of 
[virtual-serial-ports](https://www.hhdsoftware.com/virtual-serial-ports).




<p align="right">(<a href="#top">back to top</a>)</p>

## Some screens from project
- ### Starting screen
![screen](https://i.imgur.com/o35kFOq.png)
<br />
<br />
- ### Choosing COM port
![screen2](https://i.imgur.com/DrmLzhN.png)
<br />
<br />
- ### Choosing file to send
![screen3](https://i.imgur.com/5pvrSRI.png)
<br />
<br />
- ### Loading file to send
![screen4](https://i.imgur.com/jNgLKxL.png)
<br />
<br />
- ### Receiving file
![screen5](https://i.imgur.com/54hnEeO.png)
 

## Built With

* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
* [WPF](https://docs.microsoft.com/pl-pl/dotnet/desktop/wpf/?view=netdesktop-6.0)
* MVVM

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/pStrachota/X-MODEM-TELEKOM.svg?style=for-the-badge
[contributors-url]: https://github.com/pStrachota/X-MODEM-TELEKOM/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/pStrachota/X-MODEM-TELEKOM.svg?style=for-the-badge
[forks-url]: https://github.com/pStrachota/X-MODEM-TELEKOM/network/members
[stars-shield]: https://img.shields.io/github/stars/pStrachota/X-MODEM-TELEKOM.svg?style=for-the-badge
[stars-url]: https://github.com/pStrachota/X-MODEM-TELEKOM/stargazers
[issues-shield]: https://img.shields.io/github/issues/pStrachota/X-MODEM-TELEKOM.svg?style=for-the-badge
[issues-url]: https://github.com/pStrachota/X-MODEM-TELEKOM/issues
[license-shield]: https://img.shields.io/github/license/pStrachota/X-MODEM-TELEKOM.svg?style=for-the-badge
[license-url]: https://github.com/pStrachota/X-MODEM-TELEKOM/blob/master/LICENSE.txt


