import React, { useState } from 'react';
import axios from 'axios';
import CadastroUsuario from './CadastroUsuario'; // Certifique-se de que o caminho está correto

function App() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [token, setToken] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [exibirCadastro, setExibirCadastro] = useState(false);

  const handleLogin = async () => {
    try {
      const response = await axios.post('http://localhost:5000/api/Login', {
        Nome: username,
        Senha: password,
      });

      setToken(response.data.Token);
      setErrorMessage(''); 
      setSuccessMessage('Login bem-sucedido!');
    } catch (error) {
      console.error('Erro ao fazer login:', error.message);
      setErrorMessage('Credenciais inválidas'); // Define a mensagem de erro.
      setSuccessMessage(''); // Limpa a mensagem de sucesso em caso de falha.
    }
  };

  const handleCadastroSucesso = () => {
    setExibirCadastro(false); // Após o cadastro bem-sucedido, volta para o formulário de login
  };

  return (
    <div>
      <h1>React API Client</h1>
      <div>
        <label>Nome de Usuário:</label>
        <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
      </div>
      <div>
        <label>Senha:</label>
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      </div>
      <div>
        <button onClick={handleLogin}>Login</button>
        <button onClick={() => setExibirCadastro(true)}>Cadastrar</button>
      </div>
      {errorMessage && (
        <div style={{ color: 'red', marginTop: '10px' }}>
          <p>{errorMessage}</p>
        </div>
      )}
      {successMessage && (
        <div style={{ color: 'green', marginTop: '10px' }}>
          <p>{successMessage}</p>
        </div>
      )}
      {token && (
        <div>
          <h2>Token:</h2>
          <p>{token}</p>
        </div>
      )}
      {exibirCadastro && (
        <CadastroUsuario onCadastroSucesso={handleCadastroSucesso} />
      )}
    </div>
  );
}

export default App;
