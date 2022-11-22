import { Col, Container, Row } from 'reactstrap';
import { Login } from './Login';
import { useCookies } from 'react-cookie';
import { Dashboard } from './Dashboard';

export const Home = () => {
    const [cookies] = useCookies(['username']);

    return cookies.username ? <Dashboard /> : <Login /> ;
}

export default Home;