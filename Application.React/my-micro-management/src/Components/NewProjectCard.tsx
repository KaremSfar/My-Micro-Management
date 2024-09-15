import { PlusIcon } from '@heroicons/react/24/outline';
import { Dialog } from '@headlessui/react'; // Add this import
import { useState } from 'react';
import NewProjectForm from './NewProjectForm';
import { GetProjectDto } from '../DTOs/ProjectDto';

interface NewProjectCardProps {
    onProjectCreated: (project: GetProjectDto) => void;
}

function NewProjectCard({ onProjectCreated }: NewProjectCardProps) {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const handleOpenModal = () => {
        setIsModalOpen(true);
    };

    return (
        <div
            onClick={handleOpenModal}
            className="lg:aspect-[5/3] sm:min-w-48 border-2 rounded-lg shadow-md min-w-full m-1 hover:scale-[1.01] transition-transform hover:cursor-pointer bg-purple-400"
            style={{ borderColor: 'rgb(85, 0, 128)' }}>
            <div className="flex flex-col h-full justify-between font-bold p-2">
                <span className="w-full text-white">
                    Add new Activity
                </span>
                <div className="flex justify-center items-center flex-grow">
                    <div className="border-2 border-white rounded-full p-2">
                        <PlusIcon className="h-8 w-8 text-white" />
                    </div>
                </div>
                <div className="w-full"></div> {/* Empty div for spacing */}
            </div>
            <Dialog open={isModalOpen} onClose={() => setIsModalOpen(false)}>
                <Dialog.Overlay className="fixed inset-0 bg-black opacity-30" />
                <div className="fixed inset-0 flex items-center justify-center">
                    <Dialog.Panel className="bg-white rounded p-6 w-full max-w-md">
                        <Dialog.Title className="text-lg font-medium mb-4">Create New Project</Dialog.Title>
                        <NewProjectForm
                            onClose={() => setIsModalOpen(false)}
                            onProjectCreated={(project: GetProjectDto) => {
                                onProjectCreated(project);
                                setIsModalOpen(false);
                            }}
                        />
                    </Dialog.Panel>
                </div>
            </Dialog>
        </div>
    );
}

export default NewProjectCard;
