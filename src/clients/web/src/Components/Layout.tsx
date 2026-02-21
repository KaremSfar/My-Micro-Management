import Navbar from './Navbar';
import { ReactNode } from 'react';

const Layout = ({ children }: { children: ReactNode }) => {
    return (
        <div className="w-full h-full flex flex-col">
            <Navbar />
            <main className="flex-1 overflow-auto">{children}</main>
        </div>
    );
};

export default Layout;
