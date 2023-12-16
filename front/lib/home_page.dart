import 'package:agenci/driver/driver_home_page.dart';
import 'package:agenci/parking/parking_home_page.dart';
import 'package:flutter/material.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final TextEditingController _usernameController = TextEditingController();

  String _username = "";

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
      body: Column(
        children: [
          TextField(
            controller: _usernameController,
            decoration: const InputDecoration(
              labelText: "Username",
            ),
          ),
          ElevatedButton(
            onPressed: _username.isEmpty
                ? null
                : () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => DriverHomePage(
                          driverId: _username,
                        ),
                      ),
                    );
                  },
            child: const Text("Driver"),
          ),
          ElevatedButton(
            onPressed: _username.isEmpty
                ? null
                : () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) =>
                            ParkingHomePage(parkingId: _username),
                      ),
                    );
                  },
            child: const Text("Parking"),
          ),
        ],
      ),
    );
  }
}
