import { Popover } from "@headlessui/react";
import { useAuth } from "../Auth/AuthContext";

function Avatar() {
    const { logout } = useAuth();

    const handleLogout = async () => {
        await logout();
    };
    
    return (
        <Popover className="relative">
            <Popover.Button className="focus:outline-none flex items-center text-slate-600">
                <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                    <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd"></path>
                </svg>
            </Popover.Button>

            <Popover.Panel className="absolute right-0 z-50 mt-3 w-64">
                <div className="overflow-hidden rounded-lg shadow-lg ring-1 ring-black/5">
                    <div className="bg-gray-50 p-4">
                        <button
                            onClick={handleLogout}
                            className="w-full text-left flow-root rounded-md px-2 py-2 transition duration-150 ease-in-out hover:bg-gray-100 focus:outline-none focus-visible:ring focus-visible:ring-orange-500/50"
                        >
                            <span className="flex items-center">
                                <span className="text-sm font-medium text-gray-900">
                                    Logout
                                </span>
                            </span>
                            <span className="block text-sm text-gray-500">
                                Log out from the current account
                            </span>
                        </button>
                    </div>
                </div>
            </Popover.Panel>
        </Popover>
    );
}

export default Avatar;
