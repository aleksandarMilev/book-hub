import React, { useState } from 'react';
import { service } from '../services/authenticationService';

export default function Login() {
    let [username, setUsername] = useState('');
    let [password, setPassword] = useState('');
    let [error, setError] = useState('');

    let handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await service.loginAsync(username, password);
        } catch (err) {
            setError('Invalid credentials');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Login</h2>
            <input
                type="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Username"
                required
            />
            <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                required
            />
            <button type="submit">Login</button>
            {error && <p>{error}</p>}
        </form>
    );
};
