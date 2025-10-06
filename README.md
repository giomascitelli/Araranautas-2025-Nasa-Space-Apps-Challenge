<img width="512" height="231" alt="Araranauts_logo_1" src="https://github.com/user-attachments/assets/3e4e2aa2-a0a3-43ab-aa6f-1834048ed93e" />

Space Habitat Layout Creator and Management Game

## Unity
Estamos usando a versão 2022.3.26f1 do Unity para a criação do jogo.

Caso não tenha baixado essa versão ainda, você pode encontrar ela nesse link: https://unity.com/releases/editor/archive

## Branches
```main``` → Branch estável. Sempre contém o código testado e pronto para ser entregue ou demonstrado.

```develop``` → Branch de integração. Novas features são unidas aqui e testadas antes de irem para ```main```.

## Fluxo de Trabalho
1. Criar uma branch de feature ou correção
   
   Sempre crie a branch a partir da ```develop``` e nomeie a branch de forma descritiva, usando hífens:
   ```
   git checkout develop
   git pull
   git checkout -b feature-sistema-de-recursos
   ```
   Exemplos de nomes de branch:
   - ```feature-sistema-de-grid```
   - ```feature-ui-hud```
   - ```fix-bug-camera```
   - ```doc-atualizacao-readme```

2. Trabalhar na sua branch

   Faça commits com mensagens claras:
     ```
     git add .
     git commit -m "Implementa sistema basico de posicionamento no grid"
     ```
     
3. Enviar a branch e abrir um PR

   Envie sua branch para o repositório e abra um PR para a branch ```develop```. Outro desenvolvedor deve revisar o código antes do merge.

4. Testes na branch ```develop```

   Após o merge na ```develop```, o time deve testar a nova feature/fix no ambiente de integração. Caso surjam bugs, crie novas branchs a partir da ```develop``` para corrigir e depois faça o merge de volta.

5. Merge para ```main```

   Quando a ```develop``` estiver estável e totalmente testada, ela pode ser unida à ```main```. Esse merge só deve acontecer quando o projeto estiver estável.
