import Navbar from './Navbar';
import { ReactNode } from 'react';

const Layout = ({ children }: { children: ReactNode }) => {
    return (
        <div className="w-full h-full">
            <Navbar />
            <main>{children}</main>
        </div>
    );
};

export default Layout;
