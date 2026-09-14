const path = require('path');
const { LanguageClient } = require('vscode-languageclient/node');

let client;

async function activate(context) {
  const server = context.asAbsolutePath(path.join('server', 'CDSLanguageServer.dll'));
  client = new LanguageClient(
    'cds',
    'CDS Language Server',
    { command: 'dotnet', args: [server] },
    { documentSelector: [{ scheme: 'file', language: 'cds' }] }
  );
  await client.start();
}

async function deactivate() {
  await client?.stop();
}

module.exports = { activate, deactivate };
