# Marvin 
## Sistema de Gerenciamento para Centros Automotivos

Este sistema foi desenvolvido como parte do trabalho de conclusão de curso do curso de graduação em Tecnologia em Análise e Desenvolvimento de Sistemas da Universidade Federal do Paraná.

- Aluna: Giulia Guarise Gutierrez

- Aluno: Raphael Pereira Rodrigues

- Orientador: Prof. Dr. João Eugenio Marynowski

<details>
  <summary>Diagrama do sistema e Repositórios</summary>

![Diagrama do sistema](/images/marvin.png)

 - [Marvin Webapp](https://github.com/gguarise/marvin-webapp)
 - [Autenticador](https://github.com/noh4nsen/atm-autenticador-api)
 - [Atendimento](https://github.com/noh4nsen/atm-atendimento-api)
 - [Fornecedor](https://github.com/noh4nsen/atm-fornecedor-api)
 - [Clientes](https://github.com/noh4nsen/atm-clientes-api)
 - [Setup](https://github.com/noh4nsen/atm-marvin-setup)
</details>
<br>

## Instalação
### Sistema Operacional Windows 10:

1.	Abrir o prompt de comando ou o PowerShell como administrador.

2.	Instalar o WSL  (Windows Subsystem for Linux) através do comando “wsl -- install”.

3.	Reiniciar a máquina para efetivar a instalação do WSL.

4.	Acessar o site do Docker e fazer o download do Docker Desktop.

5.	Instalar o Docker Desktop.

6.	Abrir a linha de comando do WSL (Ubuntu).

7.	Navegar até a raiz da pasta setup do projeto atm-marvin-setup (ex.: cd /mnt/c/Users/{nome-do-usuario}/Desktop/[…]/atm-marvin-setup/setup).

8.	Executar o comando “docker-compose pull” para baixar as imagens das aplicações no repositório local.
![Comando para baixar as imagens.](/images/inst-8.png) 
9.	Conferir se as imagens foram baixadas corretamente através do comando “docker images”, em caso de erro executar o comando “docker-compose pull” novamente. 
![Listagem de imagens](/images/inst-9.png)
10.	Executar o comando “docker network create postgres-net” para criar uma rede de comunicação interna do Docker na qual os microsserviços se comunicarão (A criação da rede pode ser conferida através do comando “docker network ls”).
![Criação de rede interna](/images/inst-10.png) 
![Listagem de redes](/images/inst-10-2.png)
11.	Executar o comando “docker-compose up -d” para iniciar os contêineres.
![Comando para iniciar os contêineres](/images/inst-11.png) 
12.	O correto funcionamento dos contêineres pode ser verificado através do comando “docker ps”.
![Comando para listagem de contêineres ativos](/images/inst-12.png) 
13.	Efetive a importação dos certificados de desenvolvimento na pasta /atm-marvin-setup/certificates/https/.

14.	Acesse alguma das APIs (ex.: API de Clientes em https://localhost:5677) e desative os avisos de segurança para aquele domínio.

15.	Após a instalação de todos os projetos, o sistema estará disponível através do navegador no URI http://localhost.

<br>

<details>
  <summary>Desenvolvedores</summary>

 - Giulia Guarise Gutierrez [GitHub](https://github.com/gguarise)
 - Raphael Pereira Rodrigues [GitHub](https://github.com/noh4nsen)
</details>