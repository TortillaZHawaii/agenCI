import 'package:agenci/home_page.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

void main() {
  Intl.defaultLocale = "en_US";
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    const apiBaseUrl = String.fromEnvironment("API_BASE_URL");
    debugPrint("API_BASE_URL: $apiBaseUrl");

    return MaterialApp(
      title: 'AgenCI',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color.fromARGB(255, 7, 46, 108),
        ),
        useMaterial3: true,
      ),
      home: const HomePage(),
    );
  }
}
