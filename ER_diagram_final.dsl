workspace "Aarhus Space Program" "Complete ER model including Experiments" {

    model {
        group "Aarhus Space Program" {
            system = softwareSystem "Mission Management System" {
                database = container "Central Database" "Relational storage" "PostgreSQL" {
                    
                    # --- PERSONNEL ---
                    staff = component "Staff" "PK: staff_id. Name, Hire date, Paygrade" "Table"
                    manager = component "Manager" "PK: staff_id (FK). Department" "Table"
                    scientist = component "Scientist" "PK: staff_id (FK). Title, Speciality" "Table"
                    astronaut = component "Astronaut" "PK: staff_id (FK). Rank, ExpSim, ExpSpace" "Table"
                    user = component "User" "PK: Id. StaffId (FK), Username, PasswordHash" "Table"

                    # --- MISSION & HARDWARE ---
                    mission = component "Mission" "PK: Name. Launchdate, Duration, Status, Type, Rocket ID, Location, Destination, Manager ID" "Table"
                    rocket = component "Rocket" "PK: Rocket ID. Name, Weight, Fuelcap, Payload, Stages, Crewcap" "Table"
                    launchpad = component "Launchpad" "PK: Location. Status, Max weight" "Table"
                    body = component "Celestial Body" "PK: Name. Distance, Body type" "Table"

                    # --- NEW DOMAIN: EXPERIMENTS ---
                    experiment = component "Experiment" "PK: id. Name, Description, CreationDate, MissionID (FK), LeadScientistID (FK)" "Table"

                    # --- RELATIONSHIPS ---
                    
                    # Inheritance (Is-A)
                    staff -> manager "Specialized as (1:1)"
                    staff -> scientist "Specialized as (1:1)"
                    staff -> astronaut "Specialized as (1:1)"
                    Staff -> User "has account (1:1)"

                    # Business Logic
                    manager -> mission "manages (1:N)"
                    scientist -> mission "works_on (N:N)"
                    astronaut -> mission "assigned to (N:N)"
                    
                    rocket -> mission "uses (1:1)"
                    launchpad -> mission "launches from (1:N)"
                    body -> mission "targets (1:1)"
                    body -> body "orbits (0:N)"

                    # Relationships for the new Experiments domain
                    mission -> experiment "contains (1:N)"
                    scientist -> experiment "responsible for / creates (1:N)"
                    astronaut -> experiment "carries out (N:N)"
                }
            }
        }
    }

    views {
        component database "Full_ER_View" {
            include *
            autolayout lr
            description "The full conceptual ER diagram for Aarhus Space Program."
        }

        styles {
            element "Table" {
                shape Box
                background #1168bd
                color #ffffff
            }
            element "Experiment" {
                background #d43f33
                color #ffffff
            }
            element "Mission" {
                background #208050
                color #ffffff
            }
        }
    }
}