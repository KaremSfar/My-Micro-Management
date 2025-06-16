function Analytics() {
    const days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    const hours = Array.from({ length: 24 }, (_, i) => i);

    // Example events - replace with your actual events
    const events = [
        {
            id: 1,
            title: "Meeting",
            start: new Date('2024-01-01T12:33:00'),
            end: new Date('2024-01-01T14:15:00'),
            day: 0 // Monday = 0, Tuesday = 1, etc.
        },
        {
            id: 2,
            title: "Call",
            start: new Date('2024-01-02T09:45:00'),
            end: new Date('2024-01-02T10:30:00'),
            day: 1
        }
    ];

    // Calculate position and width for an event
    const getEventStyle = (event: any) => {
        const startHour = event.start.getHours();
        const startMinute = event.start.getMinutes();
        const endHour = event.end.getHours();
        const endMinute = event.end.getMinutes();

        // Convert to decimal hours (e.g., 12:33 = 12.55)
        const startDecimal = startHour + (startMinute / 60);
        const endDecimal = endHour + (endMinute / 60);

        // Calculate position as percentage (0-100%)
        const left = (startDecimal / 24) * 100;
        const width = ((endDecimal - startDecimal) / 24) * 100;

        return {
            left: `${left}%`,
            width: `${width}%`,
            top: '2px',
            bottom: '2px'
        };
    };

    return (
        <div className="w-full h-full flex items-center justify-center my-16">
            <div className="w-4/5 h-4/5">
                <table className="w-full h-full border-collapse">
                    <tbody>
                        {days.map((day, dayIndex) => (
                            <tr key={day} className="h-full relative">
                                <td className="w-12 text-gray-600 pr-2 font-medium">
                                    {day}
                                </td>
                                <td className="relative p-0" style={{ width: '100%' }}>
                                    {/* Grid cells */}
                                    <div className="flex h-full">
                                        {hours.map((hour) => (
                                            <div
                                                key={hour}
                                                className="border border-gray-200 flex-1"
                                            />
                                        ))}
                                    </div>
                                    
                                    {/* Events overlay */}
                                    {events
                                        .filter(event => event.day === dayIndex)
                                        .map(event => (
                                            <div
                                                key={event.id}
                                                className="absolute bg-blue-500 text-white text-xs px-1 rounded opacity-80 hover:opacity-100 z-10 flex items-center"
                                                style={getEventStyle(event)}
                                                title={`${event.title} (${event.start.toLocaleTimeString()} - ${event.end.toLocaleTimeString()})`}
                                            >
                                                {event.title}
                                            </div>
                                        ))
                                    }
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default Analytics;
