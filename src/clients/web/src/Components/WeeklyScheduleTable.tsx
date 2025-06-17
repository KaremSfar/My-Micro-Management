export interface TimeTableEvent {
    title: string;
    start: Date;
    end: Date;
    color: string;
}

interface WeeklyScheduleTableProps {
    events: TimeTableEvent[];
}

function WeeklyScheduleTable({ events }: WeeklyScheduleTableProps) {
    const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const hours = Array.from({ length: 24 }, (_, i) => i);

    const getEventStyle = (event: TimeTableEvent) => {
        const startHour = event.start.getHours();
        const startMinute = event.start.getMinutes();
        const endHour = event.end.getHours();
        const endMinute = event.end.getMinutes();

        const startDecimal = startHour + (startMinute / 60);
        const endDecimal = endHour + (endMinute / 60);

        // Calculate position as percentage (0-100%)
        const left = (startDecimal / 24) * 100;
        const width = ((endDecimal - startDecimal) / 24) * 100;
        const colorRgb = hexToRgb(event.color);

        return {
            left: `${left}%`,
            width: `${width}%`,
            top: '2px',
            bottom: '2px',
            backgroundColor: `rgb(${colorRgb[0]}, ${colorRgb[1]}, ${colorRgb[2]})`
        };
    };

    const hexToRgb = (hex: string): number[] => {
        const hexRgb: string = hex.replace(/^#?([a-f\d])([a-f\d])([a-f\d])$/i,
            (_, r, g, b) => '#' + r + r + g + g + b + b);

        return hexRgb
            .substring(1).match(/.{2}/g)!
            .map(x => parseInt(x, 16));
    }

    function getLastSunday(): Date {
        const today = new Date();
        const dayOfWeek = today.getDay(); // Sunday = 0, Monday = 1, ..., Saturday = 6

        // Calculate the difference in days to the last Sunday
        const diff = today.getDate() - dayOfWeek;

        const lastSunday = new Date(today.setDate(diff));

        // Reset time to midnight
        lastSunday.setHours(0, 0, 0, 0);

        return lastSunday;
    }

    events = events.filter(e => e.start > getLastSunday());

    return (
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
                                .filter(showThisDaysEvents(dayIndex))
                                .map((event, i) => (
                                    <div
                                        key={i}
                                        className="group absolute bg-blue-500 text-white text-xs px-1 rounded opacity-80 hover:opacity-100 z-10 flex items-center"
                                        style={getEventStyle(event)}
                                        title={`${event.title} (${event.start.toLocaleTimeString()} - ${event.end.toLocaleTimeString()})`}>
                                            <div className="hidden group-hover:block absolute bottom-full left-1/2 -translate-x-1/2 mb-2 w-max bg-gray-700 text-white px-2 py-1 rounded-md text-sm">
                                                {event.title}
                                            <div className="absolute top-full left-1/2 -translate-x-1/2 w-0 h-0 border-x-4 border-x-transparent border-t-4 border-t-gray-700"></div>
                                        </div>
                                    </div>
                                ))
                            }
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );

    function showThisDaysEvents(dayIndexOnTable: number): (value: TimeTableEvent, index: number, array: TimeTableEvent[]) => unknown {
        return event => event.start.getDay() === dayIndexOnTable;
    }
}

export default WeeklyScheduleTable;