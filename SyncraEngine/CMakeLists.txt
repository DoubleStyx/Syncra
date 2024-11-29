cmake_minimum_required(VERSION 3.30)
project(SyncraEngine C)

find_package(cglm CONFIG REQUIRED)
find_package(Vulkan REQUIRED)
find_package(glfw3 CONFIG REQUIRED)
find_package(OpenXR CONFIG REQUIRED)

set(CMAKE_C_STANDARD 23)

add_executable(SyncraEngine main.c)

target_link_libraries(SyncraEngine PRIVATE Vulkan::Vulkan)
target_link_libraries(SyncraEngine PRIVATE Cglm:Cglm)
target_link_libraries(SyncraEngine PRIVATE glfw)
target_link_libraries(SyncraEngine PRIVATE OpenXR::Loader)
