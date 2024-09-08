import { useAuth } from "../../Auth/AuthContext";

export const useCreateTimeSession = (projectId: string) => {
  const { accessToken } = useAuth();

  return async (seconds: number) => {
    const endDate = new Date().toISOString();
    const startTime = new Date(new Date().getTime() - seconds * 1000).toISOString();

    const body = {
      startTime,
      endDate,
      projectIds: [projectId]
    };

    try {
      const response = await fetch(`${process.env.REACT_APP_MAIN_SERVICE_BASE_URL}/api/timeSessions`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${accessToken}`
        },
        body: JSON.stringify(body)
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      const data = await response.json();
      console.log('Time session created:', data);
    } catch (error) {
      console.error('Failed to create time session:', error);
    }
  };
};