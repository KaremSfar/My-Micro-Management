import React, { useState } from 'react';
import { useAuth } from '../Auth/AuthContext';

interface NewProjectFormProps {
    onClose: () => void;
    onProjectCreated: (id: string) => void;
}

function NewProjectForm({ onClose, onProjectCreated }: NewProjectFormProps) {
    const [projectName, setProjectName] = useState('');
    const [projectColor, setProjectColor] = useState('#000000');
    const { accessToken } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/api/projects`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                },
                body: JSON.stringify({ name: projectName, color: projectColor })
            });

            if (!response.ok) {
                throw new Error('Failed to create project');
            }

            const project = await response.json();
            onProjectCreated(project.id);
            onClose();
        } catch (error) {
            console.error('Error creating project:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <label htmlFor="projectName" className="block text-sm font-medium text-gray-700">
                    Project Name
                </label>
                <input
                    type="text"
                    id="projectName"
                    value={projectName}
                    onChange={(e) => setProjectName(e.target.value)}
                    required
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                />
            </div>
            <div>
                <label htmlFor="projectColor" className="block text-sm font-medium text-gray-700">
                    Project Color
                </label>
                <input
                    type="color"
                    id="projectColor"
                    value={projectColor}
                    onChange={(e) => setProjectColor(e.target.value)}
                    className="mt-1 block w-full h-10 rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                />
            </div>
            <div className="flex justify-end space-x-2">
                <button
                    type="button"
                    onClick={onClose}
                    className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                    Cancel
                </button>
                <button
                    type="submit"
                    className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                    Create Project
                </button>
            </div>
        </form>
    );
}

export default NewProjectForm;