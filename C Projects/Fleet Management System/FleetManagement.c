//-----------------------------------------------------------------------------
//header files
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

//define constants
#define maxBoats 120
#define maxNameLength 127
#define fileNameLength 256

//defining new types for place types
typedef enum {
    slip, land, trailor, storage
} PlaceType;

//one of the identifiers
typedef union ExtraInfo {
    int slipNumber;
    char bayLetter;
    char trailorTag[20];
    int storageNumber;
} ExtraInfo;

//holding general boat info
typedef struct {
    char name[maxNameLength + 1];
    int length;
    PlaceType place;
    ExtraInfo extra;
    double amountOwed;
} Boat;

//boatmanager info
typedef struct {
    Boat* boats[maxBoats];
    int boatCount;
} BoatManager;

//-----------------------------------------------------------------------------
//calculating monthly charges
double calculateMonthlyCharge(PlaceType place, int length) {
    switch (place) {
        case slip: 
            return length * 12.5;
        case land: 
            return length * 14.0;
        case trailor: 
            return length * 25.0;
        case storage: 
            return length * 11.2;
        default: 
            return 0.0;
    }
}
//-----------------------------------------------------------------------------
//Add a boat by providing as a string that looks like one line of the .csv file
void addBoatFromString(BoatManager* manager, const char *boatData) {

    if (manager->boatCount >= maxBoats) {
        printf("Max boats reached, unable to add more.\n");
        return;
    }

    Boat* newBoat = (Boat*)malloc(sizeof(Boat));
    if (!newBoat) {
        printf("Failed to allocate memory for new boat.\n");
        return;
    }

    //scanning for the name of the boat up to 127 characters, not including a comma
    char placeStr[20];
    sscanf(boatData, "%127[^,],%d,%19[^,],%19[^,],%lf", newBoat->name, &newBoat->length, placeStr, newBoat->extra.trailorTag, &newBoat->amountOwed);
    
    //converts place string to lowercase to make input case insensitive
    for (int i = 0; placeStr[i]; i++){
        placeStr[i] = tolower(placeStr[i]);
    }
    
    //using string comparison to check for place type and then allocating memory for each attribute
    if (strcmp(placeStr, "slip") == 0) {
        newBoat->place = slip;
        sscanf(newBoat->extra.trailorTag, "%d", &newBoat->extra.slipNumber);
    } else if (strcmp(placeStr, "land") == 0) {
        newBoat->place = land;
        newBoat->extra.bayLetter = newBoat->extra.trailorTag[0];
    } else if (strcmp(placeStr, "trailor") == 0) {
        newBoat->place = trailor;
        // trailerTag is already assigned
    } else if (strcmp(placeStr, "storage") == 0) {
        newBoat->place = storage;
        sscanf(newBoat->extra.trailorTag, "%d", &newBoat->extra.storageNumber);
    } else {
        free(newBoat);
        printf("Invalid place type.\n");
        return;
    }
    //increasing boat count
    manager->boats[manager->boatCount++] = newBoat;
}
//-----------------------------------------------------------------------------
void loadBoatsFromFile(BoatManager* manager, const char *fileName) {
    FILE *file = fopen(fileName, "r");
    if (!file) {
        perror("Failed to open file");
        return;
    }

    char line[1024];
    while (fgets(line, sizeof(line), file)) {
        line[strcspn(line, "\n")] = 0; // Remove trailing newline
        addBoatFromString(manager, line);
    }

    fclose(file);
}
//-----------------------------------------------------------------------------
void saveBoatsToFile(BoatManager* manager, const char *fileName) {
    FILE *file = fopen(fileName, "w");
    if (!file) {
        perror("Failed to open file for writing");
        return;
    }

    for (int i = 0; i < manager->boatCount; i++) {
        Boat* boat = manager->boats[i];
        if (boat->place == slip) {
            fprintf(file, "%s,%d,slip,%d,%.2f\n", boat->name, boat->length, boat->extra.slipNumber, boat->amountOwed);
        } else if (boat->place == land) {
            fprintf(file, "%s,%d,land,%c,%.2f\n", boat->name, boat->length, boat->extra.bayLetter, boat->amountOwed);
        } else if (boat->place == trailor) {
            fprintf(file, "%s,%d,trailor,%s,%.2f\n", boat->name, boat->length, boat->extra.trailorTag, boat->amountOwed);
        } else if (boat->place == storage) {
            fprintf(file, "%s,%d,storage,%d,%.2f\n", boat->name, boat->length, boat->extra.storageNumber, boat->amountOwed);
        }
    }

    fclose(file);
}
//-----------------------------------------------------------------------------
void removeBoatByName(BoatManager* manager, const char *boatName) {
    for (int i = 0; i < manager->boatCount; i++) {
        if (manager->boats[i] && strcasecmp(manager->boats[i]->name, boatName) == 0) {
            free(manager->boats[i]);
            for (int j = i; j < manager->boatCount - 1; j++) {
                manager->boats[j] = manager->boats[j + 1];
            }
            manager->boatCount--;
            printf("Boat '%s' removed.\n", boatName);
            return;
        }
    }
    printf("Boat '%s' not found.\n", boatName);
}
//-----------------------------------------------------------------------------
//Accept a payment for the boat, up to the amount owed
void acceptPayment(BoatManager* manager, const char *boatName, double payment) {
    for (int i = 0; i < manager->boatCount; i++) {
        if (manager->boats[i] && strcasecmp(manager->boats[i]->name, boatName) == 0) {
            if (manager->boats[i]->amountOwed < payment) {
                printf("That is more than the amount owed, $%.2f\n", manager->boats[i]->amountOwed);
            } else {
                manager->boats[i]->amountOwed -= payment;
                printf("Payment accepted for '%s'. New amount owed: %.2f\n", boatName, manager->boats[i]->amountOwed);
            }
            return;
        }
    }
    printf("Boat '%s' not found.\n", boatName);
}
//-----------------------------------------------------------------------------
//Update the amount owed because a new month has started
void updateAmountOwed(BoatManager* manager) {
    for (int i = 0; i < manager->boatCount; i++) {
        if (manager->boats[i]) {
            double charge = calculateMonthlyCharge(manager->boats[i]->place, manager->boats[i]->length);
            manager->boats[i]->amountOwed += charge;
        }
    }
    printf("All amounts owed have been updated.\n");
}
//-----------------------------------------------------------------------------
//comparing boat names in order to sort boats by name
int compareBoatNames(const void *a, const void *b) {
    const Boat* boatA = *(const Boat**)a;
    const Boat* boatB = *(const Boat**)b;
    return strcasecmp(boatA->name, boatB->name);
}
//-----------------------------------------------------------------------------
//Print the boat inventory, sorted alphabetically by boat name
void printBoatInventory(BoatManager* manager) {
    qsort(manager->boats, manager->boatCount, sizeof(Boat*), compareBoatNames);
    for (int i = 0; i < manager->boatCount; i++) {
        Boat* boat = manager->boats[i];
        printf("%-20s %4d'", boat->name, boat->length);
        switch (boat->place) {
            case slip:
                printf("%8s %4s %2d","slip","#",boat->extra.slipNumber);
                break;
            case land:
                printf("%8s %7c","land",boat->extra.bayLetter);
                break;
            case trailor:
                printf("%8s %7s","trailor",boat->extra.trailorTag);
                break;
            case storage:
                printf("%8s %4s %2d","storage","#",boat->extra.storageNumber);
                break;
        }
        printf("   Owes $%.2f\n", boat->amountOwed);
    }
}
//-----------------------------------------------------------------------------
//menu options and executing functions for each option
void showMenu(BoatManager* manager) {
    char choice;
    char boatData[fileNameLength];
    char boatName[maxNameLength + 1]; 
    double payment;

    printf("Welcome to the Boat Management System\n");

    do {
        printf("(I)nventory, (A)dd, (R)emove, (P)ayment, (M)onth, e(X)it : ");
        scanf(" %c", &choice); 
        getchar(); 

        //making lowercase so that both uppercase & lowercase forms accepted
        switch (tolower(choice)) {
            case 'i':
                printBoatInventory(manager);
                break;
            case 'a':
                printf("Please enter the boat data in CSV format: ");
                fgets(boatData, sizeof(boatData), stdin);
                boatData[strcspn(boatData, "\n")] = 0; 
                addBoatFromString(manager, boatData);
                break;
            case 'r':
                printf("Please enter the boat name: ");
                fgets(boatName, sizeof(boatName), stdin);
                boatName[strcspn(boatName, "\n")] = 0; 
                removeBoatByName(manager, boatName);
                break;
            case 'p':
                printf("Please enter the boat name: ");
                fgets(boatName, sizeof(boatName), stdin);
                boatName[strcspn(boatName, "\n")] = 0; 
                printf("Please enter the amount to be paid: ");
                scanf("%lf", &payment);
                acceptPayment(manager, boatName, payment);
                break;
            case 'm':
                updateAmountOwed(manager);
                break;
            case 'x':
                printf("Exiting the Boat Management System\n");
                break;
            default:
                printf("Invalid option %c\n", choice);
        }
    } while (tolower(choice) != 'x');

}
//-----------------------------------------------------------------------------
int main(int argc, char *argv[]) {

    const char* fileName;
    if (argc < 2) {
        printf("Usage: %s <filename.csv>\n", argv[0]);
        return 1; // Exit the program if no filename is provided
    } else {
        fileName = argv[1]; // Use the filename provided on the command line
    }

    BoatManager manager = {0};
    loadBoatsFromFile(&manager, argv[1]);
    showMenu(&manager);
    saveBoatsToFile(&manager, fileName);

    for (int i = 0; i < manager.boatCount; i++) {
        free(manager.boats[i]);
    }

    return 0;
}
//-----------------------------------------------------------------------------

