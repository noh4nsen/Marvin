# Marvin 
## Sistema de Gerenciamento para Centros Automotivos

![GITHUB](https://user-images.githubusercontent.com/82844620/167056867-3dee8596-133f-49b5-bcdb-c42c1d3e1e08.png)

Este sistema foi desenvolvido como parte do Trabalho de Conclusão de Curso do curso de graduação em Tecnologia em Análise e Desenvolvimento de Sistemas da Universidade Federal do Paraná.

- Discentes: Giulia Guarise Gutierrez e Raphael Pereira Rodrigues

- Orientador: Prof. Dr. João Eugenio Marynowski

<details>
  <summary>Diagrama do sistema e Repositórios Individuais</summary>

![Diagrama do sistema](/images/marvin.png)

 - [Marvin Webapp](https://github.com/gguarise/marvin-webapp)
   * [Imagem no Docker hub](https://hub.docker.com/repository/docker/noh4nsen/atm-marvin-webapp)
 - [Autenticador](https://github.com/noh4nsen/atm-autenticador-api)
   * [Imagem no Docker hub](https://hub.docker.com/repository/docker/noh4nsen/atm-autenticador-api)
 - [Atendimento](https://github.com/noh4nsen/atm-atendimento-api)
   * [Imagem no Docker hub](https://hub.docker.com/repository/docker/noh4nsen/atm-atendimento-api) 
 - [Fornecedor](https://github.com/noh4nsen/atm-fornecedor-api)
   * [Imagem no Docker hub](https://hub.docker.com/repository/docker/noh4nsen/atm-fornecedor-api)  
 - [Clientes](https://github.com/noh4nsen/atm-clientes-api)
   * [Imagem no Docker hub](https://hub.docker.com/repository/docker/noh4nsen/atm-clientes-api)   
 - [Setup](https://github.com/noh4nsen/atm-marvin-setup)
</details>
<br>

## Instalação
### Sistema Operacional Windows 10:

1.	Abrir o prompt de comando ou o PowerShell como administrador.

2.	Instalar o [WSL](https://docs.microsoft.com/pt-br/windows/wsl/install)  (Windows Subsystem for Linux) através do comando “wsl -- install”.

3.	Reiniciar a máquina para efetivar a instalação do WSL.

4.	Acessar o site do Docker e fazer o download do [Docker Desktop](https://www.docker.com/products/docker-desktop/).

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
> Senha: marvin-atm
14.	Acesse alguma das APIs (ex.: API de Clientes em https://localhost:5677) e desative os avisos de segurança para aquele domínio.
> Dependendo do navegador utilizado, é preciso acessar todas as portas (5675, 5677, 5679 e 5681)
15.	Após a instalação de todos os projetos, o sistema estará disponível através do navegador no URI http://localhost.

<br>

<details>
  <summary>Desenvolvedores</summary>

 - Giulia Guarise Gutierrez [GitHub](https://github.com/gguarise)
 - Raphael Pereira Rodrigues [GitHub](https://github.com/noh4nsen)
</details>
