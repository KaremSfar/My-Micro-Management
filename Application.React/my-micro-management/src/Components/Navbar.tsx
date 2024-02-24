import { NavLink } from "react-router-dom";
import Avatar from './Avatar';

const Navbar = () => {

    const navLinkStyle = ({ isActive }: { isActive: boolean }) => {
        return isActive
            ? "p-2 bg-zinc-100 text-gray-950 block rounded-md text-base font-medium"
            : "text-gray-600 p-2 hover:bg-zinc-100 hover:text-gray-950 block rounded-md text-base font-medium";
    }

    return (
        <nav className="flex justify-between items-center bg-white">
            <div className="flex justify-between items-center shadow-md border-2 border-black rounded-xl h-14 p-2 gap-1">
                <NavLink to="/" className={navLinkStyle}>
                    <svg viewBox="0 0 24 24" className="w-6 h-6" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M1.13953 12.2767H1.63953H1.13953ZM1.13953 20.7369H0.639534H1.13953ZM20.8703 22.375V21.875V22.375ZM2.78376 22.375V22.875H2.78376L2.78376 22.375ZM22.5145 12.2767H22.0145H22.5145ZM22.5145 20.7369H23.0145H22.5145ZM21.6445 10.0567L21.2774 10.3962L21.6445 10.0567ZM14.2454 2.05623L13.8784 2.39572V2.39572L14.2454 2.05623ZM9.40863 2.05623L9.77571 2.39571L9.40863 2.05623ZM2.00959 10.0567L1.64251 9.7172V9.7172L2.00959 10.0567ZM8.53857 14.1844H9.03857H8.53857ZM8.53857 21.5559H8.03857H8.53857ZM14.2934 22.375V21.875V22.375ZM9.36069 22.375V22.875V22.375ZM15.1155 14.1844H15.6155H15.1155ZM15.1155 21.5559H14.6155H15.1155ZM14.2934 13.3653L14.2934 12.8653L14.2934 13.3653ZM9.36069 13.3653L9.36069 13.8653H9.36069L9.36069 13.3653ZM0.639534 12.2767L0.639534 20.7369H1.63953L1.63953 12.2767H0.639534ZM20.8703 21.875L2.78376 21.875L2.78376 22.875L20.8703 22.875V21.875ZM22.0145 12.2767V20.7369H23.0145V12.2767H22.0145ZM22.0116 9.71724L14.6125 1.71675L13.8784 2.39572L21.2774 10.3962L22.0116 9.71724ZM9.04154 1.71674L1.64251 9.7172L2.37667 10.3962L9.77571 2.39571L9.04154 1.71674ZM14.6125 1.71675C13.1122 0.0944223 10.5419 0.0944152 9.04154 1.71674L9.77571 2.39571C10.8802 1.20143 12.7738 1.20143 13.8784 2.39572L14.6125 1.71675ZM23.0145 12.2767C23.0145 11.3281 22.6563 10.4144 22.0116 9.71724L21.2774 10.3962C21.7516 10.9089 22.0145 11.5803 22.0145 12.2767H23.0145ZM1.63953 12.2767C1.63953 11.5802 1.90249 10.9089 2.37667 10.3962L1.64251 9.7172C0.997749 10.4144 0.639534 11.328 0.639534 12.2767H1.63953ZM0.639534 20.7369C0.639534 21.9195 1.60129 22.875 2.78376 22.875V21.875C2.15007 21.875 1.63953 21.3637 1.63953 20.7369H0.639534ZM20.8703 22.875C22.0528 22.875 23.0145 21.9195 23.0145 20.7369H22.0145C22.0145 21.3637 21.504 21.875 20.8703 21.875V22.875ZM8.03857 14.1844L8.03857 21.5559H9.03857L9.03857 14.1844H8.03857ZM14.2934 21.875H9.36069V22.875H14.2934V21.875ZM14.6155 14.1844L14.6155 21.5559H15.6155L15.6155 14.1844H14.6155ZM14.2934 12.8653L9.36069 12.8653L9.36069 13.8653L14.2934 13.8653L14.2934 12.8653ZM15.6155 14.1844C15.6155 13.4541 15.0218 12.8653 14.2934 12.8653L14.2934 13.8653C14.473 13.8653 14.6155 14.0099 14.6155 14.1844H15.6155ZM9.03857 14.1844C9.03857 14.0099 9.18103 13.8653 9.36069 13.8653L9.36069 12.8653C8.63226 12.8653 8.03857 13.4541 8.03857 14.1844H9.03857ZM8.03857 21.5559C8.03857 22.2862 8.63226 22.875 9.36069 22.875V21.875C9.18103 21.875 9.03857 21.7304 9.03857 21.5559H8.03857ZM14.2934 22.875C15.0218 22.875 15.6155 22.2862 15.6155 21.5559H14.6155C14.6155 21.7304 14.473 21.875 14.2934 21.875V22.875Z" fill="#595D62" />
                    </svg>
                </NavLink>
            </div>
            <div className="flex-grow"></div> {/* This div can act as spacer if needed */}
            <div className="flex justify-between items-center shadow-md border-2 border-black rounded-xl h-14 p-2 gap-1">
                <NavLink to="/analytics" className={navLinkStyle}>
                    <svg viewBox="0 0 40 40" className="w-6 h-6" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M0.59375 37.4062H37.4062M2.00962 17.5841L10.5048 10.5048L16.1683 14.7524L35.9903 0.59375V30.3156H2.00965L2.00962 17.5841Z" stroke="black" strokeLinecap="round" strokeLinejoin="round" />
                    </svg>
                </NavLink>
                <Avatar />
            </div>
        </nav >
    );
};

export default Navbar;
