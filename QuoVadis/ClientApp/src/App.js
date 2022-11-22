import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { Home } from './components/Home';
import { Layout } from './components/Layout';
import { Map } from './components/Map';
import { RealTimeView } from './components/RealTimeView';
import './custom.css';

export const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/map" element={<Map />} />
            </Routes>
        </Layout>
    );
};

export default App;
