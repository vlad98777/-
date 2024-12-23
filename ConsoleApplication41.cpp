#include <iostream>
#include <fstream>
#include <vector>
using namespace std;
struct Employee {
    string lastName;
    int age;
};

// Функция для добавления нового сотрудника
void addEmployee(vector<Employee>& employees) {
    Employee newEmployee;
    cout << " фамилия сотрудника: ";
    cin >> newEmployee.lastName;
    cout << " возраст сотрудника: ";
    cin >> newEmployee.age;
    employees.push_back(newEmployee);
    cout << "Сотрудник добавлен.\n";
}

// Функция для редактирования данных сотрудника
void editEmployee(vector<Employee>& employees) {
    string lastName;
    cout << " фамилия сотрудника, данные для редактирования: ";
    cin >> lastName;

    for (auto& employee : employees) {
        if (employee.lastName == lastName) {
            cout << "Введите новую фамилию сотрудника: ";
            cin >> employee.lastName;
            cout << "Введите новый возраст сотрудника: ";
            cin >> employee.age;
            cout << "Данные сотрудника обновлены.\n";
            return;
        }
    }

    cout << "сотрудник с данной фамилией " << lastName << " не найден.\n";
}

// Функция для удаления сотрудника
void deleteEmployee(vector<Employee>& employees) {
    string lastName;
    cout << "введите фамилию сотрудника, которую необходимо удалить: ";
    cin >> lastName;

    for (auto it = employees.begin(); it != employees.end(); ++it) {
        if (it->lastName == lastName) {
            employees.erase(it);
            std::cout << "сотрудник удален.\n";
            return;
        }
    }

    cout << "сотрудник с такой  фамилией " << lastName << " не найден.\n";
}

// Функция для поиска сотрудника по фамилии
void searchByLastName(const vector<Employee>& employees) {
    string lastName;
    cout << "введите фамилию сотрудника для поиска: ";
    cin >> lastName;

    for (const auto& employee : employees) {
        if (employee.lastName == lastName) {
            cout<< "Фамилия: " << employee.lastName << ", Возраст: " << employee.age << "\n";
            return;
        }
    }

    cout << "сотрудник с фамилией " << lastName << " не найден.\n";
}

// Функция для вывода информации обо всех сотрудниках
void printAllEmployees(const vector<Employee>& employees) {
    for (const auto& employee : employees) {
        cout << "Фамилия: " << employee.lastName << ", Возраст: " << employee.age << "\n";
    }
}

// Функция для сохранения списка сотрудников в файл
void saveToFile(const vector<Employee>& employees, const std::string& filename) {
    ofstream outputFile(filename);

    for (const auto& employee : employees) {
        outputFile << employee.lastName << " " << employee.age << "\n";
    }

    cout << "список сотрудников сохранен в файл " << filename << ".\n";
}

// Функция для загрузки списка сотрудников из файла
void loadFromFile(std::vector<Employee>& employees, const string& filename) {
    ifstream inputFile(filename);

    if (!inputFile) {
        cout << "не удалось открыть файл " << filename << ".\n";
        return;
    }

    employees.clear();
    string lastName;
    int age;

    while (inputFile >> lastName >> age) {
        Employee employee;
        employee.lastName = lastName;
        employee.age = age;
        employees.push_back(employee);
    }

    cout << "cписок сотрудников  " << filename << ".\n";
}

int main() {
    vector<Employee> employees;
    string filename = "employees.txt";
    setlocale(LC_ALL, "Rus");

    // Загрузка списка сотрудников из файла при старте программы
    loadFromFile(employees, filename);

    int choice;

    do {
        cout << "\nМеню:\n";
        cout << "1. Добавить сотрудника\n";
        cout << "2. Редактировать данные сотрудника\n";
        cout << "3. Удалить сотрудника\n";
        cout << "4. Поиск сотрудника по фамилии\n";
        cout << "5. Вывести информацию обо всех сотрудниках\n";
        cout << "6. Сохранить список сотрудников в файл\n";
        cout << "0. Выход\n";
        cout << "Выберите действие: ";
        cin >> choice;

        switch (choice) {
        case 1:
            addEmployee(employees);
            break;
        case 2:
            editEmployee(employees);
            break;
        case 3:
            deleteEmployee(employees);
            break;
        case 4:
            searchByLastName(employees);
            break;
        case 5:
            printAllEmployees(employees);
            break;
        case 6:
            saveToFile(employees, filename);
            break;
        case 0:
            saveToFile(employees, filename);
            cout << "Выход\n";
            break;
        default:
            cout << "Нет такой опции\n";
        }
    } while (choice != 0);

    return 0;
}