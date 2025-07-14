# DocsViewer 

**DocsViewer** é um sistema de gerenciamento e visualização de documentos digitais para ambientes regulados, com controle de permissões, relatórios e logs. 

## Funcionalidades Principais

- Login seguro com bloqueio temporário após tentativas erradas
- Permissões de acesso por usuário, categoria e subcategoria
- Visualização rápida de arquivos PDF
- Exportação de relatórios (Excel/PDF)
- Logs completos de atividades (login, logout, edição, exclusão, etc)
- Reset de senha e desbloqueio pelo administrador
- Notas da versão visíveis na tela de login
- Suporte a modo escuro e claro

---

## **Instalação do Sistema**

O DocsViewer pode ser instalado usando um **instalador automático**, facilitando o processo para todos os usuários.

### **Instalando pelo Instalador**

1. **Localize o arquivo do instalador:**  
   O arquivo normalmente se chama `Instalador_DocsViewer.exe`.

2. **Execute o instalador:**  
   Clique duas vezes no instalador e siga as instruções na tela.

3. **Escolha a pasta de instalação:**  
   O instalador sugere “C:\Arquivos de Programas\DocsViewer” por padrão, mas você pode alterar.

4. **Atalhos automáticos:**  
   Ao finalizar, o instalador cria atalhos no Menu Iniciar e, se desejar, na área de trabalho.

5. **Banco de dados:**  
   Caso use um banco compartilhado, configure o caminho após instalar (no Configurações do sistema).

### **Atualização**

- Para atualizar, basta instalar uma nova versão por cima da antiga ou substituir apenas os arquivos `.exe` e dependências.
- Lembre-se de atualizar também o arquivo `notas_versao.txt` com as novidades da nova versão.
---

## Auditoria, Validação e Suporte

- O sistema mantém registros de todas as atividades em `activity_log.txt`.
- As versões e mudanças são documentadas em `notas_versao.txt`.
- Validado conforme os requisitos do Guia ANVISA.
- Dúvidas? Consulte o Sistema da Qualidade Ortobio.

---

*Este README pode ser atualizado conforme a evolução do sistema. Consulte sempre a versão mais recente ao instalar ou atualizar o DocsViewer.*
