import Navbar from './Navbar';
import { ReactNode } from 'react';

const Layout = ({ children }: { children: ReactNode }) => {
    return (
        <div>
            <Navbar />
            <main className='border border-black lg p-2 my-2'>{children}</main>
        </div>
    );
};

export default Layout;
