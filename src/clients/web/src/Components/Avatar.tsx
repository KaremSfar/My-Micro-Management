import { Popover } from "@headlessui/react";
import { useAuth } from "../Auth/AuthContext";

function Avatar() {
    const { logout } = useAuth(); // Use the useAuth hook to access the accessToken

    const handleLogout = async () => {
        await logout();
    };
    return (
        <div className="w-full max-w-sm">
            <Popover className="relative">
                <Popover.Button className="focus:outline-none flex justify-between items-center text-gray-600 p-1 hover:bg-zinc-100 hover:text-gray-950 block rounded-md text-base font-medium">
                    <div className="relative w-8 h-8 overflow-hidden bg-gray-100 rounded-full dark:bg-gray-600">
                        <svg className="absolute w-10 h-10 text-gray-400 -left-1" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd"></path></svg>
                    </div>
                </Popover.Button>

                <Popover.Panel className="absolute left-1/2 z-50 mt-3 w-screen max-w-sm -translate-x-1/2 transform px-4 sm:px-0 lg:max-w-l">
                    <div className="overflow-hidden rounded-lg shadow-lg ring-1 ring-black/5">
                        <div className="bg-gray-50 p-4">
                            <a  onClick={handleLogout}
                                href="##"
                                className="flow-root rounded-md px-2 py-2 transition duration-150 ease-in-out hover:bg-gray-100 focus:outline-none focus-visible:ring focus-visible:ring-orange-500/50">
                                <span className="flex items-center">
                                    <span className="text-sm font-medium text-gray-900">
                                        Logout
                                    </span>
                                </span>
                                <span className="block text-sm text-gray-500">
                                    Log out from the current account
                                </span>
                            </a>
                        </div>
                    </div>
                </Popover.Panel>
            </Popover>
        </div>
    );
}

export default Avatar;
