import 'package:agenci/driver/driver_home_page.dart';
import 'package:agenci/parking/parking_home_page.dart';
import 'package:animated_toggle_switch/animated_toggle_switch.dart';
import 'package:flutter/material.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

enum LoginType { driver, parking }

class _HomePageState extends State<HomePage> {
  final TextEditingController _usernameController = TextEditingController();

  String _username = "";
  LoginType loginType = LoginType.driver;

  @override
  void initState() {
    super.initState();
    _usernameController.addListener(() {
      setState(() {
        _username = _usernameController.text;
      });
    });
  }

  @override
  void dispose() {
    _usernameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("AgenCI"),
      ),
      body: Center(
        child: SizedBox(
          width: 350,
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: TextField(
                  controller: _usernameController,
                  decoration: const InputDecoration(
                    labelText: "Username",
                  ),
                ),
              ),
              // This is for show only
              const Padding(
                padding: EdgeInsets.all(8.0),
                child: TextField(
                  obscureText: true,
                  autocorrect: false,
                  enableSuggestions: false,
                  decoration: InputDecoration(
                    labelText: "Password",
                  ),
                ),
              ),

              ButtonBar(
                children: [
                  ElevatedButton(
                    onPressed: _username.isEmpty
                        ? null
                        : () {
                            login();
                          },
                    child: const Text("Login"),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
      bottomNavigationBar: Padding(
        padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 80.0),
        child: AnimatedToggleSwitch<LoginType>.dual(
          current: loginType,
          first: LoginType.driver,
          second: LoginType.parking,
          spacing: 50.0,
          style: const ToggleStyle(
            borderColor: Colors.transparent,
            boxShadow: [
              BoxShadow(
                color: Colors.black26,
                spreadRadius: 1,
                blurRadius: 2,
                offset: Offset(0, 1.5),
              ),
            ],
          ),
          borderWidth: 5.0,
          onChanged: (b) => setState(() {
            loginType = b;
          }),
          styleBuilder: (b) => const ToggleStyle(
            indicatorColor: Color.fromARGB(255, 225, 185, 117),
          ),
          textBuilder: (value) => value == LoginType.driver
              ? const Center(child: Text('Driver'))
              : const Center(child: Text('Parking')),
        ),
      ),
    );
  }

  void login() {
    Navigator.push(
      context,
      MaterialPageRoute(
          builder: (context) => loginType == LoginType.driver
              ? DriverHomePage(driverId: _username)
              : ParkingHomePage(parkingId: _username)),
    );
  }
}
