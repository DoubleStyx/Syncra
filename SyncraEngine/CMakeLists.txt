cmake_minimum_required(VERSION 3.30)
project(SyncraEngine C)

find_package(cglm REQUIRED)
find_package(Vulkan REQUIRED)
find_package(glfw3 REQUIRED)
find_package(OpenXR REQUIRED)

set(CMAKE_C_STANDARD 23)

add_executable(SyncraEngine main.c)

target_link_libraries(SyncraEngine PRIVATE Vulkan::Vulkan)
target_link_libraries(SyncraEngine PRIVATE cglm) # Or Cglm::Cglm if applicable
target_link_libraries(SyncraEngine PRIVATE glfw)
target_link_libraries(SyncraEngine PRIVATE OpenXR::OpenXR)
