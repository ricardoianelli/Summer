using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io
{
    public static class IoMappings
    {
        public static Dictionary<ProfibusInputId, IoData> InputMappings { get; private set; }
        public static Dictionary<ProfibusOutputId, IoData> OutputMappings { get; private set; }

        static IoMappings()
        {
            Console.WriteLine("Initializing IoMappings");
            
            // SYSTEM BLOCK INPUTS
            InputMappings = new Dictionary<ProfibusInputId, IoData>
            {
                { ProfibusInputId.USER_DOOR_NOT_CLOSED, new IoData { Node = 20, Offset = 0 } },
                { ProfibusInputId.FRONT_DOOR_NOT_CLOSED, new IoData { Node = 20, Offset = 2 } },
                { ProfibusInputId.REAR_DOOR_NOT_CLOSED, new IoData { Node = 20, Offset = 4 } },
                { ProfibusInputId.UPSTREAM_TRAY_AVAILABLE, new IoData { Node = 20, Offset = 7 } },
                { ProfibusInputId.DOWNSTREAM_ACCEPT_TRAY, new IoData { Node = 20, Offset = 8 } },
                { ProfibusInputId.SURGE_OK, new IoData { Node = 20, Offset = 15 } },
                { ProfibusInputId.SEED_DROP1_COUNTER_VALUE, new IoData { Node = 20, Offset = 3, IsAnalog = true } },
                { ProfibusInputId.SEED_DROP2_COUNTER_VALUE, new IoData { Node = 20, Offset = 6, IsAnalog = true } },
                { ProfibusInputId.SEED_DROP3_COUNTER_VALUE, new IoData { Node = 20, Offset = 9, IsAnalog = true } },
                { ProfibusInputId.SEED_DROP4_COUNTER_VALUE, new IoData { Node = 20, Offset = 12, IsAnalog = true } },
                { ProfibusInputId.SEED_DROP5_COUNTER_VALUE, new IoData { Node = 20, Offset = 15, IsAnalog = true } },
                { ProfibusInputId.SEED_DROP6_COUNTER_VALUE, new IoData { Node = 20, Offset = 18, IsAnalog = true } },

                // SAFETY BLOCK INPUTS
                { ProfibusInputId.ESTOP_USER_DOOR_NOT_PRESSED, new IoData { Node = 31, Offset = 0 } },
                { ProfibusInputId.ESTOP_INFEED_NOT_PRESSED, new IoData { Node = 31, Offset = 2 } },
                { ProfibusInputId.ESTOP_REAR_NOT_PRESSED, new IoData { Node = 31, Offset = 4 } },
                { ProfibusInputId.ESTOP_UPSTREAM_NOT_PRESSED, new IoData { Node = 31, Offset = 6 } },
                { ProfibusInputId.ESTOP_DOWNSTREAM_NOT_PRESSED, new IoData { Node = 31, Offset = 7 } },
                { ProfibusInputId.USER_DOOR_CLOSED_LOCKED, new IoData { Node = 31, Offset = 8 } },
                { ProfibusInputId.FRONT_DOOR_CLOSED_LOCKED, new IoData { Node = 31, Offset = 10 } },
                { ProfibusInputId.REAR_DOOR_CLOSED_LOCKED, new IoData { Node = 31, Offset = 12 } },
                { ProfibusInputId.USER_DOOR_PUSHBUTTON, new IoData { Node = 31, Offset = 19 } },
                { ProfibusInputId.FRONT_DOOR_PUSHBUTTON, new IoData { Node = 31, Offset = 20 } },
                { ProfibusInputId.REAR_DOOR_PUSHBUTTON, new IoData { Node = 31, Offset = 21 } },
                { ProfibusInputId.BYPASS_STATE, new IoData { Node = 31, Offset = 22 } },
                { ProfibusInputId.ESTOP_RELAYS_GOOD, new IoData { Node = 31, Offset = 96 } },
                { ProfibusInputId.DOOR_RELAYS_GOOD, new IoData { Node = 31, Offset = 98 } },
                { ProfibusInputId.RESET_NOT_NEEDED, new IoData { Node = 31, Offset = 201 } },

                // ABOVE DECK INPUTS
                { ProfibusInputId.VSTACK_OPLATE_MOVER_IMAGING, new IoData { Node = 41, Offset = 40 } },
                { ProfibusInputId.VSTACK_OPLATE_MOVER_VSTACK, new IoData { Node = 41, Offset = 41 } },
                { ProfibusInputId.OPLATE_IMAGING_1_DETECTED, new IoData { Node = 41, Offset = 42 } },
                { ProfibusInputId.OPLATE_IMAGING_2_DETECTED, new IoData { Node = 41, Offset = 43 } },
                { ProfibusInputId.IMAGING_OPLATE_UNCLAMPED, new IoData { Node = 41, Offset = 44 } },
                { ProfibusInputId.IMAGING_OPLATE_CLAMPED, new IoData { Node = 41, Offset = 45 } },
                { ProfibusInputId.DELIDDER_DISCARD, new IoData { Node = 41, Offset = 48 } },
                { ProfibusInputId.DELIDDER_PICKUP, new IoData { Node = 41, Offset = 49 } },
                { ProfibusInputId.DELIDDER_HAS_LID, new IoData { Node = 41, Offset = 50 } },
                { ProfibusInputId.VSTACK_OPLATE_TOO_TALL, new IoData { Node = 41, Offset = 51 } },
                { ProfibusInputId.FLIPPER_OPLATE_MOVER_IMAGING, new IoData { Node = 41, Offset = 52 } },
                { ProfibusInputId.FLIPPER_OPLATE_MOVER_FLIPPER, new IoData { Node = 41, Offset = 53 } },
                { ProfibusInputId.ON_WHEEL_SENSOR, new IoData { Node = 41, Offset = 54 } },
                { ProfibusInputId.FLIPPER_COVER_CLOSED, new IoData { Node = 41, Offset = 56 } },
                { ProfibusInputId.FLIPPER_COVER_OPEN, new IoData { Node = 41, Offset = 57 } },
                { ProfibusInputId.FLIPPER_UNCLAMPED, new IoData { Node = 41, Offset = 58 } },
                { ProfibusInputId.FLIPPER_CLAMPED, new IoData { Node = 41, Offset = 59 } },
                { ProfibusInputId.FLIPPER_LOAD, new IoData { Node = 41, Offset = 60 } },
                { ProfibusInputId.FLIPPER_DUMP, new IoData { Node = 41, Offset = 61 } },
                { ProfibusInputId.FLIPPER_HAS_OPLATE, new IoData { Node = 41, Offset = 62 } },
                { ProfibusInputId.FLIPPER_SEED_DUMP_CLOSED, new IoData { Node = 41, Offset = 63 } },
                { ProfibusInputId.ABOVE_REVOLVER_GATE_CLOSED, new IoData { Node = 41, Offset = 64 } },
                { ProfibusInputId.ABOVE_REVOLVER_GATE_OPENED, new IoData { Node = 41, Offset = 65 } },
                { ProfibusInputId.DIVERTER_RIGHT_GATE_CLOSED, new IoData { Node = 41, Offset = 66 } },
                { ProfibusInputId.DIVERTER_RIGHT_GATE_OPENED, new IoData { Node = 41, Offset = 67 } },
                { ProfibusInputId.DIVERTER_LEFT_GATE_CLOSED, new IoData { Node = 41, Offset = 68 } },
                { ProfibusInputId.DIVERTER_LEFT_GATE_OPENED, new IoData { Node = 41, Offset = 69 } },
                { ProfibusInputId.DROP_TUBES_DOWN, new IoData { Node = 41, Offset = 70 } },
                { ProfibusInputId.DROP_TUBES_UP, new IoData { Node = 41, Offset = 71 } },
                { ProfibusInputId.VSTACK_OPLATE_HEIGHT_VALUE, new IoData { Node = 41, Offset = 2, IsAnalog = true } },
                { ProfibusInputId.SINGULATOR_VACUUM_PRESSURE, new IoData { Node = 41, Offset = 0, IsAnalog = true } },

                // BELOW DECK INPUTS
                { ProfibusInputId.SHUTTLE_GATE_1_CLOSED, new IoData { Node = 42, Offset = 60 } },
                { ProfibusInputId.SHUTTLE_GATE_1_OPENED, new IoData { Node = 42, Offset = 61 } },
                { ProfibusInputId.SHUTTLE_GATE_2_CLOSED, new IoData { Node = 42, Offset = 62 } },
                { ProfibusInputId.SHUTTLE_GATE_2_OPENED, new IoData { Node = 42, Offset = 63 } },
                { ProfibusInputId.SHUTTLE_GATE_3_CLOSED, new IoData { Node = 42, Offset = 64 } },
                { ProfibusInputId.SHUTTLE_GATE_3_OPENED, new IoData { Node = 42, Offset = 65 } },
                { ProfibusInputId.SHUTTLE_GATE_4_CLOSED, new IoData { Node = 42, Offset = 66 } },
                { ProfibusInputId.SHUTTLE_GATE_4_OPENED, new IoData { Node = 42, Offset = 67 } },
                { ProfibusInputId.SHUTTLE_GATE_5_CLOSED, new IoData { Node = 42, Offset = 68 } },
                { ProfibusInputId.SHUTTLE_GATE_5_OPENED, new IoData { Node = 42, Offset = 69 } },
                { ProfibusInputId.SHUTTLE_GATE_6_CLOSED, new IoData { Node = 42, Offset = 70 } },
                { ProfibusInputId.SHUTTLE_GATE_6_OPENED, new IoData { Node = 42, Offset = 71 } },
                { ProfibusInputId.SEED_DISCARD_CONTAINER_PRESENT, new IoData { Node = 42, Offset = 74 } },
                { ProfibusInputId.INFEED_TRAY_ENTRY, new IoData { Node = 42, Offset = 40 } },
                { ProfibusInputId.INFEED_TRAY_EXIT, new IoData { Node = 42, Offset = 41 } },
                { ProfibusInputId.MAIN_CONVEYOR_ENTRY, new IoData { Node = 42, Offset = 43 } },
                { ProfibusInputId.MAIN_CONVEYOR_DROP_TUBES, new IoData { Node = 42, Offset = 44 } },
                { ProfibusInputId.MAIN_CONVEYOR_EXIT, new IoData { Node = 42, Offset = 45 } },
                { ProfibusInputId.QUEUE_BIN_LEFT_CLOSED, new IoData { Node = 42, Offset = 46 } },
                { ProfibusInputId.QUEUE_BIN_RIGHT_CLOSED, new IoData { Node = 42, Offset = 47 } },
                { ProfibusInputId.QUEUE_BIN_LEFT_LOCKED, new IoData { Node = 42, Offset = 48 } },
                { ProfibusInputId.QUEUE_BIN_LEFT_UNLOCKED, new IoData { Node = 42, Offset = 49 } },
                { ProfibusInputId.QUEUE_BIN_LEFT_GATE_CLOSED, new IoData { Node = 42, Offset = 50 } },
                { ProfibusInputId.QUEUE_BIN_LEFT_GATE_OPENED, new IoData { Node = 42, Offset = 51 } },
                { ProfibusInputId.QUEUE_BIN_RIGHT_LOCKED, new IoData { Node = 42, Offset = 52 } },
                { ProfibusInputId.QUEUE_BIN_RIGHT_UNLOCKED, new IoData { Node = 42, Offset = 53 } },
                { ProfibusInputId.QUEUE_BIN_RIGHT_GATE_CLOSED, new IoData { Node = 42, Offset = 54 } },
                { ProfibusInputId.QUEUE_BIN_RIGHT_GATE_OPENED, new IoData { Node = 42, Offset = 55 } },
                { ProfibusInputId.DROP_TUBE_ID_SENSOR_1, new IoData { Node = 42, Offset = 56 } },
                { ProfibusInputId.DROP_TUBE_ID_SENSOR_2, new IoData { Node = 42, Offset = 57 } },
                { ProfibusInputId.DROP_TUBE_ID_SENSOR_3, new IoData { Node = 42, Offset = 58 } }
            };

            // SYSTEM BLOCK OUTPUTS
            OutputMappings = new Dictionary<ProfibusOutputId, IoData>
            {
                { ProfibusOutputId.SINGULATION_VACUUM_ON, new IoData { Node = 20, Offset = 1 } },
                { ProfibusOutputId.INFEED_CONVEYOR, new IoData { Node = 20, Offset = 2 } },
                { ProfibusOutputId.INFEED_CONVEYOR_REVERSE, new IoData { Node = 20, Offset = 3 } },
                { ProfibusOutputId.IMAGE_LIGHT_ON, new IoData { Node = 20, Offset = 8 } },
                { ProfibusOutputId.RECEIVE_TRAY, new IoData { Node = 20, Offset = 14 } },
                { ProfibusOutputId.SEND_TRAY, new IoData { Node = 20, Offset = 15 } },
                { ProfibusOutputId.USER_DOOR_LOCK, new IoData { Node = 20, Offset = 9 } },
                { ProfibusOutputId.FRONT_DOOR_LOCK, new IoData { Node = 20, Offset = 10 } },
                { ProfibusOutputId.REAR_DOOR_LOCK, new IoData { Node = 20, Offset = 11 } },

                // SAFETY BLOCK OUTPUTS
                { ProfibusOutputId.USER_DOOR_REQUEST_LIGHT, new IoData { Node = 31, Offset = 8 } },
                { ProfibusOutputId.FRONT_DOOR_REQUEST_LIGHT, new IoData { Node = 31, Offset = 10 } },
                { ProfibusOutputId.REAR_DOOR_REQUEST_LIGHT, new IoData { Node = 31, Offset = 12 } },

                // ABOVE DECK OUTPUTS
                { ProfibusOutputId.VSTACK_OPLATE_MOVER_IMAGING, new IoData { Node = 41, Offset = 40 } },
                { ProfibusOutputId.VSTACK_OPLATE_MOVER_VSTACK, new IoData { Node = 41, Offset = 42 } },
                { ProfibusOutputId.IMAGING_CLAMPED, new IoData { Node = 41, Offset = 44 } },
                { ProfibusOutputId.DELIDDER_PICKUP, new IoData { Node = 41, Offset = 48 } },
                { ProfibusOutputId.DELIDDER_VACUUM, new IoData { Node = 41, Offset = 50 } },
                { ProfibusOutputId.FLIPPER_OPLATE_MOVER_FLIPPER, new IoData { Node = 41, Offset = 52 } },
                { ProfibusOutputId.STACKER_AIR, new IoData { Node = 41, Offset = 54 } },
                { ProfibusOutputId.FLIPPER_COVER_OPEN, new IoData { Node = 41, Offset = 56 } },
                { ProfibusOutputId.FLIPPER_CLAMPED, new IoData { Node = 41, Offset = 58 } },
                { ProfibusOutputId.FLIPPER_LOAD, new IoData { Node = 41, Offset = 60 } },
                { ProfibusOutputId.FLIPPER_DUMP, new IoData { Node = 41, Offset = 62 } },
                { ProfibusOutputId.DROP_TUBES_DOWN, new IoData { Node = 41, Offset = 64 } },
                { ProfibusOutputId.ABOVE_REVOLVER_GATE_OPEN, new IoData { Node = 41, Offset = 66 } },
                { ProfibusOutputId.DIVERTER_RIGHT_GATE_OPEN, new IoData { Node = 41, Offset = 68 } },
                { ProfibusOutputId.DIVERTER_LEFT_GATE_OPEN, new IoData { Node = 41, Offset = 70 } },

                // BELOW DECK OUTPUTS
                { ProfibusOutputId.BULK_LEFT_GREEN_LIGHT, new IoData { Node = 42, Offset = 32 } },
                { ProfibusOutputId.BULK_LEFT_YELLOW_LIGHT, new IoData { Node = 42, Offset = 33 } },
                { ProfibusOutputId.BULK_LEFT_RED_LIGHT, new IoData { Node = 42, Offset = 34 } },
                { ProfibusOutputId.BULK_RIGHT_GREEN_LIGHT, new IoData { Node = 42, Offset = 35 } },
                { ProfibusOutputId.BULK_RIGHT_YELLOW_LIGHT, new IoData { Node = 42, Offset = 36 } },
                { ProfibusOutputId.BULK_RIGHT_RED_LIGHT, new IoData { Node = 42, Offset = 37 } },
                { ProfibusOutputId.MIGRATION_EXAIR_ON, new IoData { Node = 42, Offset = 40 } },
                { ProfibusOutputId.REVOLVER_DUMP_EXAIR_ON, new IoData { Node = 42, Offset = 42 } },
                { ProfibusOutputId.DIVERTER_EXAIR_ON, new IoData { Node = 42, Offset = 44 } },
                { ProfibusOutputId.BULK_LEFT_DOOR_LOCK, new IoData { Node = 42, Offset = 48 } },
                { ProfibusOutputId.BULK_LEFT_GATE_OPEN, new IoData { Node = 42, Offset = 50 } },
                { ProfibusOutputId.BULK_RIGHT_DOOR_LOCK, new IoData { Node = 42, Offset = 52 } },
                { ProfibusOutputId.BULK_RIGHT_GATE_OPEN, new IoData { Node = 42, Offset = 54 } },
                { ProfibusOutputId.SHUTTLE_GATE_1_OPEN, new IoData { Node = 42, Offset = 60 } },
                { ProfibusOutputId.SHUTTLE_GATE_2_OPEN, new IoData { Node = 42, Offset = 62 } },
                { ProfibusOutputId.SHUTTLE_GATE_3_OPEN, new IoData { Node = 42, Offset = 64 } },
                { ProfibusOutputId.SHUTTLE_GATE_4_OPEN, new IoData { Node = 42, Offset = 66 } },
                { ProfibusOutputId.SHUTTLE_GATE_5_OPEN, new IoData { Node = 42, Offset = 68 } },
                { ProfibusOutputId.SHUTTLE_GATE_6_OPEN, new IoData { Node = 42, Offset = 70 } },

                // Analog Output
                { ProfibusOutputId.SINGULATION_VACUUM_SPEED, new IoData { Node = 42, Offset = 0, IsAnalog = true } }
            };
        }
        
        public static IoData GetIoDataById(int id)
        {
            if (Enum.IsDefined(typeof(ProfibusInputId), id))
            {
                return InputMappings[(ProfibusInputId)id];
            }

            if (Enum.IsDefined(typeof(ProfibusOutputId), id))
            {
                return OutputMappings[(ProfibusOutputId)id];
            }

            return null;
        }
    }
}

